using Microsoft.EntityFrameworkCore;
using SaaSUniversity.Server.Models;

namespace SaaSUniversity.Server.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Class> Classes { get; set; }
    }

}
