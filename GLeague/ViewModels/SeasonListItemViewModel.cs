using GLeague.Models;

namespace GLeague.ViewModels
{
    public class SeasonListItemViewModel
    {
        public required Season Season { get; set; }

        public int TotalRegistrations { get; set; }
        public int PendingRegistrations { get; set; }
        public int ApprovedRegistrations { get; set; }
        public int WaitlistedRegistrations { get; set; }
    }
}
