namespace pro.Models.Faculty.Law
{
    public class LawFacultyListViewModel
    {
        public List<LawFaculty> Faculties { get; set; } = new();
        public string SelectedSpecialization { get; set; } = "All";
        public List<string> Specializations { get; set; } = new()
{
"Constitutional Law", "Criminal Law", "Corporate Law", "International Law", "Family Law"
};
    }
}