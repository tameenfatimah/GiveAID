namespace pro.Models.Academics.Undergraduate
{
    public class UndergraduateProgram
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Degree { get; set; } = string.Empty; // BS, BA, BBA, etc.
        public string Department { get; set; } = string.Empty;
        public int DurationInYears { get; set; }
        public string Description { get; set; } = string.Empty;
        public List<string> Majors { get; set; } = new();
        public string AdmissionRequirements { get; set; } = string.Empty;
        public decimal TuitionFee { get; set; }
        public int TotalCreditHours { get; set; }
    }
}