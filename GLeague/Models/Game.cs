namespace GLeague.Models
{
    public class Game
    {
        public int Id { get; set; }

        public int DivisionId { get; set; }
        public Division Division { get; set; }

        public int HomeTeamId { get; set; }
        public Team HomeTeam { get; set; }

        public int AwayTeamId { get; set; }
        public Team AwayTeam { get; set; }

        public DateTime StartTime { get; set; }

        public GameStatus Status { get; set; } = GameStatus.Scheduled;

        public int? VenueId { get; set; }
        public Venue Venue { get; set; }

        public int? HomeScore { get; set; }
        public int? AwayScore { get; set; }

        public bool IsPlayoff { get; set; }
        public string Notes { get; set; }

        public ICollection<PlayerGameStat> PlayerStats { get; set; } = new List<PlayerGameStat>();

    }
}
