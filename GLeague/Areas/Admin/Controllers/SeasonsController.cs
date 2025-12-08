using GLeague.Data;
using GLeague.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
                .ToListAsync();

            return View(seasons);
        }
    }
}
