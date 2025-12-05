namespace GLeague.Models
{
    public class SeasonRegistration
    {
        public int Id { get; set; }

        public int SeasonId { get; set; }
        public required Season Season { get; set; }

        public required string PlayerId { get; set; }
        public required ApplicationUser Player { get; set; }

        public RegistrationStatus Status { get; set; } = RegistrationStatus.Pending;

        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;
        public DateTime? ApprovedOn { get; set; }
        public DateTime? CancelledOn { get; set; }

        public string? Notes { get; set; }
    }
}
