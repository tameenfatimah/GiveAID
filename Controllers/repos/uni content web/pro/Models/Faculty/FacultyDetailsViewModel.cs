
namespace pro.Models.Faculty
{
    public class FacultyDetailsViewModel
    {
        public Faculty Faculty { get; set; } = new();
        public List<string> Courses { get; set; } = new();
        public List<string> Publications { get; set; } = new();
        public string OfficeLocation { get; set; } = string.Empty;
        public string OfficeHours { get; set; } = string.Empty;
    }
}