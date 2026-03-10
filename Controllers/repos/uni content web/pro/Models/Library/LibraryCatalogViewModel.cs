namespace pro.Models.Library
{
    public class LibraryCatalogViewModel
    {
        public List<Book> Books { get; set; } = new();
        public string SearchQuery { get; set; } = string.Empty;
        public string SelectedCategory { get; set; } = "All";
    }
}