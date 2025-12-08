using GLeague.Data;
using GLeague.Models;
using GLeague.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLeague.Controllers
{
    [Authorize]
    public class RegistrationsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public RegistrationsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            var now = DateTime.UtcNow;

            var openSeasons = await _context.Seasons
                .Where(s => !s.IsLocked && s.RegistrationOpens <= now && s.RegistrationCloses >= now)
                .OrderBy(s => s.RegistrationCloses)
                .ToListAsync();

            var myRegistrations = await _context.SeasonRegistrations
                .Where(r => r.PlayerId == userId)
                .Include(r => r.Season)
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();

            var registrationCounts = await _context.SeasonRegistrations
                .Where(r => openSeasons.Select(s => s.Id).Contains(r.SeasonId))
                .GroupBy(r => r.SeasonId)
                .Select(g => new { SeasonId = g.Key, Count = g.Count() })
                .ToListAsync();

            var openSeasonCards = openSeasons
                .Select(season => new RegistrationCardViewModel
                {
                    Season = season,
                    ExistingRegistration = myRegistrations.FirstOrDefault(r => r.SeasonId == season.Id),
                    TotalRegistrations = registrationCounts.FirstOrDefault(c => c.SeasonId == season.Id)?.Count ?? 0
                })
                .ToList();

            var viewModel = new RegistrationDashboardViewModel
            {
                OpenSeasons = openSeasonCards,
                MyRegistrations = myRegistrations
            };

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(int seasonId, string? notes)
        {
            var userId = _userManager.GetUserId(User);
            if (userId == null)
            {
                return Challenge();
            }

            var now = DateTime.UtcNow;
            var season = await _context.Seasons
                .FirstOrDefaultAsync(s => s.Id == seasonId && !s.IsLocked && s.RegistrationOpens <= now && s.RegistrationCloses >= now);

            if (season == null)
            {
                return NotFound();
            }

            var existing = await _context.SeasonRegistrations
                .FirstOrDefaultAsync(r => r.SeasonId == seasonId && r.PlayerId == userId);

            if (existing != null)
            {
                TempData["Message"] = "You are already registered for this season.";
                return RedirectToAction(nameof(Index));
            }

            var registration = new SeasonRegistration
            {
                SeasonId = seasonId,
                PlayerId = userId,
                Status = RegistrationStatus.Pending,
                Notes = notes
            };

            _context.SeasonRegistrations.Add(registration);
            await _context.SaveChangesAsync();

            TempData["Message"] = "Registration submitted. A manager will review and assign you to a team.";
            return RedirectToAction(nameof(Index));
        }
    }
}
