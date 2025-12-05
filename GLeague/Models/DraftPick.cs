namespace GLeague.Models
{
    public class DraftPick
    {
        public int Id { get; set; }

        public int DraftId { get; set; }
        public Draft Draft { get; set; }

        public int RoundNumber { get; set; }
        public int PickNumberInRound { get; set; }
        public int OverallPickNumber { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public string PlayerId { get; set; }
        public ApplicationUser Player { get; set; }

        public DateTime MadeAt { get; set; } = DateTime.UtcNow;
    }
}
