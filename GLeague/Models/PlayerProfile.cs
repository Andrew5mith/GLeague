using System.ComponentModel.DataAnnotations;

namespace GLeague.Models
{
    public class PlayerProfile
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;

        [Required, MaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required, MaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [Phone]
        [MaxLength(20)]
        public string? PhoneNumber { get; set; }

        public ExperienceLevel ExperienceLevel { get; set; } = ExperienceLevel.None;

        public PlayerPosition PreferredPosition { get; set; } = PlayerPosition.Unknown;

        public JerseySize JerseySize { get; set; } = JerseySize.Unknown;

        [MaxLength(20)]
        public string? JerseyName { get; set; }

        [MaxLength(4)]
        public string? JerseyNumber { get; set; }

        public int? HeightInInches { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? WeightInPounds { get; set; }

        [MaxLength(100)]
        public string? CityOfResidence { get; set; }

        public string? Bio { get; set; }

        public string? PhotoFileName { get; set; }
    }
}
