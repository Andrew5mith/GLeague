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
    public class TeamsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeamsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: /Admin/Teams?seasonId=1
        public async Task<IActionResult> Index(int? seasonId)
        {
            var seasons = await _context.Seasons
                .OrderBy(s => s.Year)
                .ThenBy(s => s.Name)
                .ToListAsync();

            var teamsQuery = _context.Teams
                .Include(t => t.Season) // if you have a Season nav property
                .AsQueryable();

            if (seasonId.HasValue && seasonId.Value > 0)
            {
                teamsQuery = teamsQuery.Where(t => t.SeasonId == seasonId.Value);
            }

            var vm = new AdminTeamsIndexViewModel
            {
                Seasons = seasons,
                Teams = await teamsQuery
                    .OrderBy(t => t.Name)
                    .ToListAsync(),
                SelectedSeasonId = seasonId
            };

            return View(vm);
        }

        // GET: /Admin/Teams/Create?seasonId=1
        [HttpGet]
        public async Task<IActionResult> Create(int? seasonId)
        {
            await PopulateSeasonsDropDownList(seasonId);

            var team = new Team
            {
                SeasonId = seasonId ?? 0
            };

            return View(team);
        }

        // POST: /Admin/Teams/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Team team)
        {
            if (!ModelState.IsValid)
            {
                await PopulateSeasonsDropDownList(team.SeasonId);
                return View(team);
            }

            _context.Add(team);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index), new { seasonId = team.SeasonId });
        }

        // GET: /Admin/Teams/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            var team = await _context.Teams.FindAsync(id);
            if (team == null)
            {
                return NotFound();
            }

            await PopulateSeasonsDropDownList(team.SeasonId);
            return View(team);
        }

        // POST: /Admin/Teams/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Team team)
        {
            if (id != team.Id)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                await PopulateSeasonsDropDownList(team.SeasonId);
                return View(team);
            }

            try
            {
                _context.Update(team);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TeamExists(team.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToAction(nameof(Index), new { seasonId = team.SeasonId });
        }

        private bool TeamExists(int id)
        {
            return _context.Teams.Any(e => e.Id == id);
        }

        private async Task PopulateSeasonsDropDownList(int? selectedSeasonId = null)
        {
            var seasons = await _context.Seasons
                .OrderBy(s => s.Year)
                .ThenBy(s => s.Name)
                .ToListAsync();

            ViewData["SeasonOptions"] = seasons
                .Select(s => new
                {
                    s.Id,
                    Label = $"{s.Name} ({s.Year})"
                })
                .ToList();

            ViewData["SelectedSeasonId"] = selectedSeasonId;
        }
    }
}
