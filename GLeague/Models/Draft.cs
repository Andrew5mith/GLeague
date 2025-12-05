namespace GLeague.Models
{
    public class Draft
    {
        public int Id { get; set; }

        public int DivisionId { get; set; }
        public Division? Division { get; set; }

        public DateTime ScheduledFor { get; set; }
        public DraftStatus Status { get; set; } = DraftStatus.NotScheduled;

        public ICollection<DraftPick> Picks { get; set; } = new List<DraftPick>();
    }
}
