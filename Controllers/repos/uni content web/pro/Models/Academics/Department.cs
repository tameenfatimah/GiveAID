namespace pro.Models.Academics
{
    public class Department
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string HeadOfDepartment { get; set; } = string.Empty;
        public int TotalFaculty { get; set; }
        public int TotalStudents { get; set; }
    }
}