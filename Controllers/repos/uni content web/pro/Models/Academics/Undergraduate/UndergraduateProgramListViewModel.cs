namespace pro.Models.Academics.Undergraduate
{
    public class UndergraduateProgramListViewModel
    {
        public List<UndergraduateProgram> Programs { get; set; } = new();
        public string SelectedDegree { get; set; } = "All";
        public string SelectedDepartment { get; set; } = "All";
        public List<string> Degrees { get; set; } = new() { "BS", "BA", "BBA", "BSc" };
        public List<string> Departments { get; set; } = new();
    }
}