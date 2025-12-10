using System.Collections.Generic;

namespace GLeague.Models
{
    public class AdminTeamsIndexViewModel
    {
        public IEnumerable<Team> Teams { get; set; } = new List<Team>();
        public IEnumerable<Season> Seasons { get; set; } = new List<Season>();
        public int? SelectedSeasonId { get; set; }
    }
}
