using System.ComponentModel.DataAnnotations;
using System.Collections.Generic; // ✅ Namespace Add Karo

namespace CodeSavvyAsp.Models
{
    public class Course
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public string Instructor { get; set; } // ✅ Yahan "Instructor" property add kar do!

        public ICollection<EnrolledCourse> EnrolledCourses { get; set; } = new List<EnrolledCourse>();
    }
}
