using System.ComponentModel.DataAnnotations.Schema;

namespace luxur.Models
{
    [Table("Contact")]
    public class Contact
    {
        public int Id { get; set; }
        public required string Fname { get; set; }

        public required string Lname { get; set; }
        public required string Email { get; set; }
        public required string Message { get; set; }
    }
}
