namespace GLeague.Models
{
    public class Division
    {
        public int Id { get; set; }

        public string Name { get; set; }             // "Men's A", "Co-ed Rec"
        public int SeasonId { get; set; }
        public Season Season { get; set; }

        public int? MinAge { get; set; }
        public int? MaxAge { get; set; }
        public string SkillLevel { get; set; }       // "Rec", "Competitive", etc.

        public ICollection<Team> Teams { get; set; } = new List<Team>();
        public ICollection<Game> Games { get; set; } = new List<Game>();
        public ICollection<Draft> Drafts { get; set; } = new List<Draft>();
    }
}
