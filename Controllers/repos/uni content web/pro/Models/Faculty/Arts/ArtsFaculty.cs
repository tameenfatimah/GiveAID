namespace pro.Models.Faculty.Arts
{
    public class ArtsFaculty : Faculty
    {
        public string ArtDiscipline { get; set; } = string.Empty; // Literature, History, Philosophy, etc.
        public List<string> ResearchInterests { get; set; } = new();
        public List<string> Publications { get; set; } = new();
    }
}