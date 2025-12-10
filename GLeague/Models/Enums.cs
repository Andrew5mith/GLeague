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

    public enum PlayerPosition
    {
        Unknown = 0,
        Guard = 1,
        Forward = 2,
        Center = 3
    }

    public enum ExperienceLevel
    {
        None = 0,
        HighSchool = 1,
        Rec = 2,
        College = 3,
        Pro = 4
    }

    public enum JerseySize
    {
        Unknown = 0,
        XS = 1,
        S = 2,
        M = 3,
        L = 4,
        XL = 5,
        XXL = 6,
        XXXL = 7
    }

    public enum GameStatus
    {
        Scheduled = 0,
        InProgress = 1,
        Final = 2,
        Cancelled = 3
    }
}
