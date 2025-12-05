using Microsoft.AspNetCore.Identity;

namespace GLeague.Models
{
    public class ApplicationUser : IdentityUser
    {
        // Navigation to player profile
        public PlayerProfile PlayerProfile { get; set; }

        // User can be on multiple teams across seasons
        public ICollection<TeamMembership> TeamMemberships { get; set; } = new List<TeamMembership>();

        // Teams this user is the captain for
        public ICollection<Team> CaptainedTeams { get; set; } = new List<Team>();

        // Soft-delete / archive if someone stops playing
        public bool IsArchived { get; set; }
    }
}
