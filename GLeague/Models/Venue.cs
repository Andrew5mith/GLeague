namespace GLeague.Models
{
    public class Venue
    {
        public int Id { get; set; }

        public string Name { get; set; }          // "Main Rec Center"
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }

        public string Notes { get; set; }

        public ICollection<Game> Games { get; set; } = new List<Game>();
    }
}
