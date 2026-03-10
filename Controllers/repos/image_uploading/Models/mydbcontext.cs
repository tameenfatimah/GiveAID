using Microsoft.EntityFrameworkCore;

namespace image_uploading.Models
{
    public class Mydbcontext : DbContext
    {
        public Mydbcontext(DbContextOptions<Mydbcontext> options) : base(options)
        {
        }
        public DbSet<Product> Products { get; set; }
    }
}
