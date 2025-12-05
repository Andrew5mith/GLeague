namespace GLeague.Models
{
    public class Season
    {
        public int Id { get; set; }

        public string Name { get; set; }         // "Winter 2026"
        public int Year { get; set; }            // 2026

        public DateTime RegistrationOpens { get; set; }
        public DateTime RegistrationCloses { get; set; }

        public DateTime? DraftDate { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        public int MaxTeams { get; set; } = 0;           // 0 = unlimited
        public int MaxPlayersPerTeam { get; set; } = 0;  // 0 = unlimited

        public bool IsCurrent { get; set; }
        public bool IsLocked { get; set; }       // lock once games start

        public ICollection<Division> Divisions { get; set; } = new List<Division>();
    }
}
