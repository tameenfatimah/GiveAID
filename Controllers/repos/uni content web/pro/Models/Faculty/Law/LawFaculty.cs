namespace pro.Models.Faculty.Law
{
    public class LawFaculty : Faculty
    {
        public string LawSpecialization { get; set; } = string.Empty; // Constitutional, Criminal, etc.
        public string BarLicenseNumber { get; set; } = string.Empty;
        public bool IsLicensedAttorney { get; set; }
        public List<string> LegalExpertise { get; set; } = new();
    }
}