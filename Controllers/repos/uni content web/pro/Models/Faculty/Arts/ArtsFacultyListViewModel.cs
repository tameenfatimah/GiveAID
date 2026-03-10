
namespace pro.Models.Faculty.Arts
{
    public class ArtsFacultyListViewModel
    {
        public List<ArtsFaculty> Faculties { get; set; } = new();
        public string SelectedDiscipline { get; set; } = "All";
        public List<string> Disciplines { get; set; } = new()
{
"Literature", "History", "Philosophy", "Languages", "Fine Arts"
};
    }
}