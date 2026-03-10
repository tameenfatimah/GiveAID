namespace pro.Models.About
{
    public class LeadershipViewModel
    {
        public List<Leader> Leaders { get; set; } = new();
    }
    public class Leader
    {
        public string Name { get; set; } = string.Empty;
        public string Position { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
    }
}