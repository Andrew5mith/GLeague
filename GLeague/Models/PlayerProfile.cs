namespace GLeague.Models
{
    public class PlayerProfile
    {
        public int Id { get; set; }

        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }

        public DateTime? DateOfBirth { get; set; }

        public int? HeightInInches { get; set; }
        public int? WeightInPounds { get; set; }

        public PlayerPosition PreferredPosition { get; set; }

        public string Bio { get; set; }

        // Relative path/filename under wwwroot/images/players, etc.
        public string PhotoFileName { get; set; }
    }
}
