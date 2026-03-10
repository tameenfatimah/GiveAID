namespace pro.Models.Faculty.Medical
{
    public class MedicalFaculty : Faculty
    {
        public string MedicalSpecialty { get; set; } = string.Empty; // Surgery, Pediatrics, etc.
        public string LicenseNumber { get; set; } = string.Empty;
        public bool IsPracticing { get; set; }
        public List<string> ClinicalExpertise { get; set; } = new();
    }
}