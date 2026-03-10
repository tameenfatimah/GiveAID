
namespace pro.Models.Faculty.Medical
{
    public class MedicalFacultyListViewModel
    {
        public List<MedicalFaculty> Faculties { get; set; } = new();
        public string SelectedSpecialty { get; set; } = "All";
        public List<string> Specialties { get; set; } = new()
{
"Surgery", "Pediatrics", "Internal Medicine", "Cardiology", "Neurology"
};
    }
}