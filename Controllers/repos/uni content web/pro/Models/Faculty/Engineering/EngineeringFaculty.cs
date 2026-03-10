namespace pro.Models.Faculty.Engineering
{
    public class EngineeringFaculty : Faculty
    {
        public string EngineeringField { get; set; } = string.Empty; // Mechanical, Civil, etc.
        public string ProfessionalRegistration { get; set; } = string.Empty;
        public List<string> ResearchProjects { get; set; } = new();
        public List<string> Patents { get; set; } = new();
    }
}