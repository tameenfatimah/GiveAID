namespace pro.Models.News
{
    public class NewsListViewModel
    {
        public List<News> NewsList { get; set; } = new();
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
    }
}