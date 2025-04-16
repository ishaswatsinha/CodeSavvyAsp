using CodeSavvyAsp.Models;
using Microsoft.EntityFrameworkCore;
namespace CodeSavvyAsp.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }


        public DbSet<Student> Students { get; set; }


        //for the instructor
        public DbSet<Instructor> Instructors { get; set; }


        public DbSet<InstructorCourse> InstructorCourses { get; set; }

    }
}
