namespace GLeague.Models
{
    public class SeasonRegistration
    {
        public int Id { get; set; }

        public int SeasonId { get; set; }
        public Season Season { get; set; } = null!;

        public string PlayerId { get; set; } = null!;
        public ApplicationUser Player { get; set; } = null!;

        public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedOn { get; set; }
        public DateTime? CancelledOn { get; set; }

        public string? Notes { get; set; }
    }
}
