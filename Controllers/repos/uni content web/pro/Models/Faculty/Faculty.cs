namespace pro.Models.Faculty
{
    public class Faculty
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string FacultyType { get; set; } = string.Empty; // Arts, Medical, Law, Business, Engineering
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Designation { get; set; } = string.Empty;
        public string Qualification { get; set; } = string.Empty;
        public string Specialization { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
    }
}