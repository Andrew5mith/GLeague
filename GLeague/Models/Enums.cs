namespace GLeague.Models
{
    public enum RegistrationStatus
    {
        Pending = 0,
        Approved = 1,
        Waitlisted = 2,
        Cancelled = 3
    }

    public enum DraftStatus
    {
        NotScheduled = 0,
        Scheduled = 1,
        InProgress = 2,
        Completed = 3
    }

    public enum GameStatus
    {
        Scheduled = 0,
        InProgress = 1,
        Final = 2,
        Cancelled = 3
    }

    public enum PlayerPosition
    {
        Unknown = 0,
        Guard = 1,
        Forward = 2,
        Center = 3
    }
}
