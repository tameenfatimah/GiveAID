
namespace pro.Models.Contact
{
    public class ContactInfoViewModel
    {
        public string Address { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public List<DepartmentContact> DepartmentContacts { get; set; } = new();
    }
    public class DepartmentContact
    {
        public string Department { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}