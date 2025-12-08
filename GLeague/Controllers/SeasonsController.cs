using GLeague.Data;
using GLeague.Models;
using GLeague.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLeague.Controllers
{
    [Authorize]
    public class SeasonsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SeasonsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [AllowAnonymous]
        public async Task<IActionResult> Index()
        {
            var seasons = await _context.Seasons
                .OrderByDescending(s => s.StartDate ?? s.RegistrationOpens)
                .ToListAsync();

            var registrationCounts = await _context.SeasonRegistrations
                .GroupBy(r => r.SeasonId)
                .Select(g => new
                {
                    SeasonId = g.Key,
                    Total = g.Count(),
                    Pending = g.Count(r => r.Status == RegistrationStatus.Pending),
                    Approved = g.Count(r => r.Status == RegistrationStatus.Approved),
                    Waitlisted = g.Count(r => r.Status == RegistrationStatus.Waitlisted)
                })
                .ToListAsync();

            var viewModel = seasons
                .Select(season =>
                {
                    var counts = registrationCounts.FirstOrDefault(c => c.SeasonId == season.Id);
                    return new SeasonListItemViewModel
                    {
                        Season = season,
                        TotalRegistrations = counts?.Total ?? 0,
                        PendingRegistrations = counts?.Pending ?? 0,
                        ApprovedRegistrations = counts?.Approved ?? 0,
                        WaitlistedRegistrations = counts?.Waitlisted ?? 0
                    };
                })
                .ToList();

            return View(viewModel);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Details(int id)
        {
            var season = await _context.Seasons
                .Include(s => s.Divisions)
                    .ThenInclude(d => d.Teams)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (season == null)
            {
                return NotFound();
            }

            var registrations = await _context.SeasonRegistrations
                .Where(r => r.SeasonId == id)
                .Include(r => r.Player)
                    .ThenInclude(p => p.PlayerProfile)
                .OrderByDescending(r => r.CreatedOn)
                .ToListAsync();

            var viewModel = new SeasonDetailsViewModel
            {
                Season = season,
                Registrations = registrations
            };

            return View(viewModel);
        }

        [Authorize(Roles = "Manager")]
        public IActionResult Create()
        {
            var defaultStart = DateTime.UtcNow.Date.AddDays(30);
            var defaultEnd = defaultStart.AddMonths(2);

            var season = new Season
            {
                Name = "New Season",
                Year = DateTime.UtcNow.Year,
                RegistrationOpens = DateTime.UtcNow.Date,
                RegistrationCloses = DateTime.UtcNow.Date.AddDays(14),
                DraftDate = defaultStart.AddDays(-7),
                StartDate = defaultStart,
                EndDate = defaultEnd
            };

            return View(season);
        }

        [Authorize(Roles = "Manager")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Year,RegistrationOpens,RegistrationCloses,DraftDate,StartDate,EndDate,MaxTeams,MaxPlayersPerTeam,IsCurrent,IsLocked")] Season season)
        {
            if (season.RegistrationCloses < season.RegistrationOpens)
            {
                ModelState.AddModelError(nameof(season.RegistrationCloses), "Registration close date must be after open date.");
            }

            if (ModelState.IsValid)
            {
                _context.Add(season);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Details), new { id = season.Id });
            }

            return View(season);
        }
    }
}
