namespace pro.Models.Alumni
{
    public class AlumniListViewModel
    {
        public List<Alumni> AlumniList { get; set; } = new();
        public int SelectedYear { get; set; }
        public List<int> GraduationYears { get; set; } = new();
    }
}