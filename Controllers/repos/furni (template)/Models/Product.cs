using System.ComponentModel.DataAnnotations;

namespace furni.Models
{
    public class Product
    {
        public int Id { get; set; }
        public required string Name { get; set; }
        public required string ImagePath { get; set; }
    }
}
