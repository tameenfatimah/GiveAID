namespace pro.Models.Alumni
{
    public class Alumni
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int GraduationYear { get; set; }
        public string Degree { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public string CurrentPosition { get; set; } = string.Empty;
        public string Company { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}