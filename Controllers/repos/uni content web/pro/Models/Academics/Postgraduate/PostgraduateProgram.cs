
namespace pro.Models.Academics.Postgraduate
{
    public class PostgraduateProgram
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty; // Masters, PhD
        public string Department { get; set; } = string.Empty;
        public int DurationInYears { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> Specializations { get; set; } = new();
        public string AdmissionRequirements { get; set; } = string.Empty;
        public decimal TuitionFee { get; set; }
        public bool IsResearchBased { get; set; }
    }
}