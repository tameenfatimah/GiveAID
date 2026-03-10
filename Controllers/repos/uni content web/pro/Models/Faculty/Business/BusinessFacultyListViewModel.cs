namespace pro.Models.Faculty.Business
{
    public class BusinessFacultyListViewModel
    {
        public List<BusinessFaculty> Faculties { get; set; } = new();
        public string SelectedArea { get; set; } = "All";
        public List<string> BusinessAreas { get; set; } = new()
{
"Finance", "Marketing", "Management", "Accounting", "Entrepreneurship"
};
    }
}