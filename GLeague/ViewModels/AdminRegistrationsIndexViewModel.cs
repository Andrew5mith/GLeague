using GLeague.Models;
using System.Collections.Generic;

namespace GLeague.ViewModels
{
    public class AdminRegistrationsIndexViewModel
    {
        public int? SelectedSeasonId { get; set; }
        public bool ShowOnlyPending { get; set; } = true;

        public IList<Season> Seasons { get; set; } = new List<Season>();
        public IList<SeasonRegistration> Registrations { get; set; } = new List<SeasonRegistration>();
    }
}
