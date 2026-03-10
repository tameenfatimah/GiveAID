using Microsoft.EntityFrameworkCore;
using Project2.Models;

namespace Project2.Data
{
    public class Mydbcontext : DbContext
    {
        public Mydbcontext(DbContextOptions<Mydbcontext> options):base(options)
        {
            
        }
        public DbSet<Product> products { get; set; }
    }
}
