using GLeague.Data;
using GLeague.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLeague.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager")]
    public class PlayerRegistrationsController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly UserManager<ApplicationUser> _userManager;

        public PlayerRegistrationsController(ApplicationDbContext db, UserManager<ApplicationUser> userManager)
        {
            _db = db;
            _userManager = userManager;
        }

        // GET: /Admin/PlayerRegistrations
        public async Task<IActionResult> Index()
        {
            // Pending = EmailConfirmed == false and not archived
            var pendingPlayers = await _db.Users
                .Include(u => u.PlayerProfile)
                .Where(u => !u.EmailConfirmed && !u.IsArchived)
                .OrderBy(u => u.PlayerProfile.FirstName)
                .ThenBy(u => u.PlayerProfile.LastName)
                .ToListAsync();

            return View(pendingPlayers);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.EmailConfirmed = true;    // this is what allows login (because RequireConfirmedAccount = true)
            user.IsArchived = false;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Could not approve player. See logs for details.";
                return RedirectToAction(nameof(Index));
            }

            var profileName = user.PlayerProfile != null
                ? $"{user.PlayerProfile.FirstName} {user.PlayerProfile.LastName}".Trim()
                : user.Email ?? "Player";

            TempData["Message"] = $"Approved {profileName}. They can now sign in.";
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deny(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            // Simple approach: soft-delete by archiving but leaving EmailConfirmed = false
            user.IsArchived = true;

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                TempData["Error"] = "Could not deny player. See logs for details.";
                return RedirectToAction(nameof(Index));
            }

            var profileName = user.PlayerProfile != null
                ? $"{user.PlayerProfile.FirstName} {user.PlayerProfile.LastName}".Trim()
                : user.Email ?? "Player";

            TempData["Message"] = $"Denied/archived {profileName}.";
            return RedirectToAction(nameof(Index));
        }
    }
}
