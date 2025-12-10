using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GLeague.Models
{
    public class Team
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; } = string.Empty;

        public string? ShortName { get; set; }
        public string? ColorPrimary { get; set; }
        public string? ColorSecondary { get; set; }

        public string? CaptainId { get; set; }
        public ApplicationUser? Captain { get; set; }

        [Required]
        public int SeasonId { get; set; }
        public Season? Season { get; set; }

        public bool IsArchived { get; set; }

        public ICollection<TeamMembership> Members { get; set; } = new List<TeamMembership>();
        public ICollection<Game> HomeGames { get; set; } = new List<Game>();
        public ICollection<Game> AwayGames { get; set; } = new List<Game>();
    }
}
