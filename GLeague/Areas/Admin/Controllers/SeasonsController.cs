using System;
using System.Linq;
using System.Threading.Tasks;
using GLeague.Data;
using GLeague.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GLeague.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Manager")]
    public class SeasonsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public SeasonsController(ApplicationDbContext db)
        {
            _db = db;
        }

        // GET: /Admin/Seasons
        public async Task<IActionResult> Index()
        {
            var seasons = await _db.Seasons
                .OrderByDescending(s => s.StartDate)
                .ThenByDescending(s => s.Year)
                .ToListAsync();

            return View(seasons);
        }

        // GET: /Admin/Seasons/Create
        public IActionResult Create()
        {
            var currentYear = DateTime.UtcNow.Year;

            var model = new Season
            {
                Year = currentYear,
                RegistrationOpens = DateTime.UtcNow.Date,
                RegistrationCloses = DateTime.UtcNow.Date.AddMonths(1),
                StartDate = DateTime.UtcNow.Date.AddMonths(2),
                EndDate = DateTime.UtcNow.Date.AddMonths(4),
                MaxTeams = 0,
                MaxPlayersPerTeam = 0
            };

            return View(model);
        }

        // POST: /Admin/Seasons/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Season season)
        {
            if (!ModelState.IsValid)
            {
                return View(season);
            }

            await NormalizeCurrentSeasonAsync(season);

            _db.Seasons.Add(season);
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        // GET: /Admin/Seasons/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var season = await _db.Seasons.FindAsync(id);
            if (season == null)
            {
                return NotFound();
            }

            return View(season);
        }

        // POST: /Admin/Seasons/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Season season)
        {
            if (id != season.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                return View(season);
            }

            try
            {
                await NormalizeCurrentSeasonAsync(season);

                _db.Update(season);
                await _db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await SeasonExists(season.Id))
                {
                    return NotFound();
                }

                throw;
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> SeasonExists(int id)
        {
            return await _db.Seasons.AnyAsync(e => e.Id == id);
        }

        // If this season is marked current, clear IsCurrent on all others
        private async Task NormalizeCurrentSeasonAsync(Season season)
        {
            if (!season.IsCurrent)
            {
                return;
            }

            var otherSeasons = await _db.Seasons
                .Where(s => s.Id != season.Id && s.IsCurrent)
                .ToListAsync();

            foreach (var s in otherSeasons)
            {
                s.IsCurrent = false;
            }
        }
    }
}
