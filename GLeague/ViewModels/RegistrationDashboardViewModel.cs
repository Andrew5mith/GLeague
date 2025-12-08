using GLeague.Models;

namespace GLeague.ViewModels
{
    public class RegistrationDashboardViewModel
    {
        public List<RegistrationCardViewModel> OpenSeasons { get; set; } = new();
        public List<SeasonRegistration> MyRegistrations { get; set; } = new();
    }
}
