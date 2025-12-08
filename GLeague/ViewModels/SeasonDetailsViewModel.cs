using GLeague.Models;

namespace GLeague.ViewModels
{
    public class SeasonDetailsViewModel
    {
        public required Season Season { get; set; }

        public List<SeasonRegistration> Registrations { get; set; } = new();
    }
}
