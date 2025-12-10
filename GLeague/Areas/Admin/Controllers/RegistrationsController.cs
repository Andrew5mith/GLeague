using System;
using System.Linq;
using System.Threading.Tasks;
using GLeague.Data;
using GLeague.Models;
using GLeague.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLeague.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager")]
    public class RegistrationsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RegistrationsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Registrations?seasonId=1&showOnlyPending=true
        public async Task<IActionResult> Index(int? seasonId, bool showOnlyPending = true)
        {
            var seasons = await _context.Seasons
                .OrderBy(s => s.Year)
                .ThenBy(s => s.Name)
                .ToListAsync();

            var query = _context.SeasonRegistrations
                .Include(r => r.Season)
                .Include(r => r.Player)              // ApplicationUser
                    .ThenInclude(u => u.PlayerProfile) // assuming this nav exists
                .AsQueryable();

            if (seasonId.HasValue && seasonId.Value > 0)
            {
                query = query.Where(r => r.SeasonId == seasonId.Value);
            }

            if (showOnlyPending)
            {
                query = query.Where(r => r.Status == RegistrationStatus.Pending);
            }

            var vm = new AdminRegistrationsIndexViewModel
            {
                SelectedSeasonId = seasonId,
                ShowOnlyPending = showOnlyPending,
                Seasons = seasons,
                Registrations = await query
                    .OrderByDescending(r => r.CreatedOn)
                    .ToListAsync()
            };

            return View(vm);
        }

        // POST: /Admin/Registrations/Approve/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve(int id)
        {
            var registration = await _context.SeasonRegistrations
                .Include(r => r.Season)
                .Include(r => r.Player)
                    .ThenInclude(u => u.PlayerProfile)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registration == null)
            {
                return NotFound();
            }

            if (registration.Status == RegistrationStatus.Approved)
            {
                // already approved; just go back
                return RedirectToAction(nameof(Index), new { seasonId = registration.SeasonId });
            }

            // If PlayerProfile is created during front-end registration,
            // you probably don't need to do anything else here besides approval.
            // If not, this is where you'd create/populate it.

            registration.Status = RegistrationStatus.Approved;
            registration.ApprovedOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { seasonId = registration.SeasonId });
        }

        // Optional: Cancel/Reject
        // POST: /Admin/Registrations/Cancel/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(int id)
        {
            var registration = await _context.SeasonRegistrations
                .FirstOrDefaultAsync(r => r.Id == id);

            if (registration == null)
            {
                return NotFound();
            }

            registration.Status = RegistrationStatus.Cancelled;
            registration.CancelledOn = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { seasonId = registration.SeasonId });
        }
    }
}
