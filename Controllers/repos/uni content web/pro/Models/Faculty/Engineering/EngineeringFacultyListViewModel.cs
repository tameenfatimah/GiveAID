namespace pro.Models.Faculty.Engineering
{
    public class EngineeringFacultyListViewModel
    {
        public List<EngineeringFaculty> Faculties { get; set; } = new();
        public string SelectedField { get; set; } = "All";
        public List<string> EngineeringFields { get; set; } = new()
{
"Computer Engineering", "Mechanical Engineering", "Civil Engineering",
"Electrical Engineering", "Chemical Engineering"
};
    }
}