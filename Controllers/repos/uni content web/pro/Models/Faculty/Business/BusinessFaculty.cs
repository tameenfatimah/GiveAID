namespace pro.Models.Faculty.Business
{
    public class BusinessFaculty : Faculty
    {
        public string BusinessArea { get; set; } = string.Empty; // Finance, Marketing, etc.
        public string Industry { get; set; } = string.Empty;
        public int YearsOfIndustryExperience { get; set; }
        public List<string> Certifications { get; set; } = new();
    }
}