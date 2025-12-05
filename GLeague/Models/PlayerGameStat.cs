namespace GLeague.Models
{
    public class PlayerGameStat
    {
        public int Id { get; set; }

        public int GameId { get; set; }
        public Game Game { get; set; }

        public string PlayerId { get; set; }
        public ApplicationUser Player { get; set; }

        public int Points { get; set; }
        public int Rebounds { get; set; }
        public int Assists { get; set; }
        public int Steals { get; set; }
        public int Blocks { get; set; }
        public int Turnovers { get; set; }
        public int Fouls { get; set; }

        public int FieldGoalsMade { get; set; }
        public int FieldGoalsAttempted { get; set; }
        public int ThreePointersMade { get; set; }
        public int ThreePointersAttempted { get; set; }
        public int FreeThrowsMade { get; set; }
        public int FreeThrowsAttempted { get; set; }

        public int MinutesPlayed { get; set; }
    }
}
