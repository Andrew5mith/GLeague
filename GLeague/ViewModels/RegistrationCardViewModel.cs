using GLeague.Models;

namespace GLeague.ViewModels
{
    public class RegistrationCardViewModel
    {
        public required Season Season { get; set; }
        public SeasonRegistration? ExistingRegistration { get; set; }

        public int TotalRegistrations { get; set; }
    }
}
