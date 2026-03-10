namespace pro.Models.News
{
    public class News
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime PublishedDate { get; set; }
        public string Author { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
    }
}