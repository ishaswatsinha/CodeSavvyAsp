using Microsoft.EntityFrameworkCore;
using CodeSavvyAsp.net.Models;

namespace CodeSavvyAsp.net.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Student> Students { get; set; }


        //for the instructor
        public DbSet<Instructor> Instructors { get; set; }
    }
}
