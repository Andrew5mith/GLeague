namespace GLeague.Models
{
    public class Team
    {
        public int Id { get; set; }

        public string Name { get; set; }             // "Downtown Ballers"
        public string ShortName { get; set; }        // "DTB"
        public string ColorPrimary { get; set; }     // "#123456"
        public string ColorSecondary { get; set; }

        public int DivisionId { get; set; }
        public Division Division { get; set; }

        public string CaptainId { get; set; }
        public ApplicationUser Captain { get; set; }

        public bool IsArchived { get; set; }

        public ICollection<TeamMembership> Members { get; set; } = new List<TeamMembership>();

        public ICollection<Game> HomeGames { get; set; } = new List<Game>();
        public ICollection<Game> AwayGames { get; set; } = new List<Game>();
    }
}
