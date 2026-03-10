using System.ComponentModel.DataAnnotations;

namespace furni.Models
{
    public class Contact
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string Email { get; set; }
        public required string Message { get; set; }
    }
}
