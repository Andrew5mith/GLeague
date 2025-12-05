namespace GLeague.Models
{
    public class TeamMembership
    {
        public int Id { get; set; }

        public int TeamId { get; set; }
        public Team Team { get; set; }

        public string PlayerId { get; set; }
        public ApplicationUser Player { get; set; }

        public bool IsCaptain { get; set; }
        public bool IsActive { get; set; } = true;

        public int JerseyNumber { get; set; }

        public DateTime JoinedOn { get; set; } = DateTime.UtcNow;
        public DateTime? LeftOn { get; set; }
    }
}
