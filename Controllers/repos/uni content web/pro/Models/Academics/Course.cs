namespace pro.Models.Academics
{
    public class Course
    {
        public int Id { get; set; }
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string Department { get; set; } = string.Empty;
        public string Level { get; set; } = string.Empty; // Undergraduate or Postgraduate
        public string Instructor { get; set; } = string.Empty;
    }
}