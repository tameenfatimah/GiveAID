using Login_Register.Models;
using Microsoft.EntityFrameworkCore;

namespace Login_Register.Data
{
    public class Mydbcontext : DbContext
    {
        public Mydbcontext(DbContextOptions<Mydbcontext> options) : base(options)
        {
        }
        public DbSet<Users> Users { get; set; }
    }
}
