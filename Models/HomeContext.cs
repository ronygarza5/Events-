
using Microsoft.EntityFrameworkCore;
namespace Exam.Models
{
    public class HomeContext : DbContext 
    {
        public HomeContext(DbContextOptions options) : base(options) {}

        public DbSet<User> Users {get; set;}
        public DbSet<Plan> Plans {get; set;}
        public DbSet<Rsvp> Rsvps {get; set;}
    }
}