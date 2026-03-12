namespace GiveAID.Models
{
    public class Event
    {
        public int EventId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }    
        public DateTime EventDate { get; set; }
        public string Location { get; set; }
        public string ImagePath { get; set; }   
        public bool IsUpcoming { get; set; }    //true = upcoming event, false = past event
        public DateTime CreatedAt { get; set; }   
    }
}
