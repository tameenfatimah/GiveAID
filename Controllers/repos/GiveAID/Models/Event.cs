namespace GiveAID.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }    // e.g., "Education", "Children", "Disabled"
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string ImagePath { get; set; }   // Path to the event image
        public bool IsUpcoming { get; set; }    //true = > upcoming event, false => past event
        public DateTime CreatedAt { get; set; }   //Time when Admin added the event
    }
}
