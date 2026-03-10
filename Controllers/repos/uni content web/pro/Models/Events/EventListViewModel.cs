namespace pro.Models.Events
{
    public class EventListViewModel
    {
        public List<Event> UpcomingEvents { get; set; } = new();
        public List<Event> PastEvents { get; set; } = new();
        public string SelectedCategory { get; set; } = "All";
    }
}