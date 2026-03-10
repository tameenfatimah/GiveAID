namespace pro.Models.Events
{
    public class Event
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime EventDate { get; set; }
        public string Location { get; set; } = string.Empty;
        public string Organizer { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}