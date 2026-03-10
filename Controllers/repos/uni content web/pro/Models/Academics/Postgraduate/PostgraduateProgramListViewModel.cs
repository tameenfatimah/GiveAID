namespace pro.Models.Academics.Postgraduate
{
    public class PostgraduateProgramListViewModel
    {
        public List<PostgraduateProgram> Programs { get; set; } = new();
        public string SelectedDegree { get; set; } = "All";
        public string SelectedDepartment { get; set; } = "All";
        public List<string> Degrees { get; set; } = new() { "Masters", "PhD", "MPhil" };
        public List<string> Departments { get; set; } = new();
    }
}