using CodeSavvyAsp.Models;
using Microsoft.EntityFrameworkCore;

namespace CodeSavvyAsp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // ✅ Students Table
        public DbSet<Student> Students { get; set; }

        // ✅ Instructors Table
        public DbSet<Instructor> Instructors { get; set; }

        public DbSet<InstructorCourse> InstructorCourses { get; set; }

        // ✅ Enrolled Courses Table (Properly Linked)
        public DbSet<EnrolledCourse> EnrolledCourses { get; set; }

        // ✅ Courses Table (for referencing in EnrolledCourses)
        public DbSet<Course> Courses { get; set; }

        public DbSet<Enrollments> Enrollments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // ✅ Ensure EnrolledCourse has Foreign Key to Students Table
            modelBuilder.Entity<EnrolledCourse>()
                .HasOne(e => e.Student)
                .WithMany(s => s.EnrolledCourses)  // 👈 Add navigation property in `Student` model
                .HasForeignKey(e => e.StudentId)
                .OnDelete(DeleteBehavior.Cascade);  // ✅ Cascade delete when Student is removed

            // ✅ Ensure EnrolledCourse has Foreign Key to Courses Table
            modelBuilder.Entity<EnrolledCourse>()
                .HasOne(e => e.Course)
                .WithMany(c => c.EnrolledCourses)  // 👈 Add navigation property in `Course` model
                .HasForeignKey(e => e.CourseId)
                .OnDelete(DeleteBehavior.Cascade);  // ✅ Cascade delete when Course is removed
        }
    }
}
