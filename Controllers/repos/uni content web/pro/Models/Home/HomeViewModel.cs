namespace pro.Models.Home
{
    public class HomeViewModel
    {
        public string WelcomeMessage { get; set; } = string.Empty;
        public List<string> Announcements { get; set; } = new();
        public List<UpcomingEvent> UpcomingEvents { get; set; } = new();
    }
    public class UpcomingEvent
    {
        public string Title { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public string Location { get; set; } = string.Empty;
    }
}