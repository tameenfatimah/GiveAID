using Microsoft.EntityFrameworkCore;

namespace GiveAID.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<Query> Queries { get; set; }
        public DbSet<Gallery> Galleries { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<NGO> NGOs { get; set; }
        public DbSet<Partner> Partners { get; set; }
        public DbSet<ContactMessage> ContactMessages { get; set; }
    }
}
