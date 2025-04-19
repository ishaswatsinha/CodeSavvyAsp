using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace CodeSavvyAsp.Models
{
    public class Enrollments
    {
        [Key]
        public int Id { get; set; }  // ✅ Auto-increment Primary Key

        [ForeignKey("Student")]
        public int StudentId { get; set; }  // ✅ Student Foreign Key

        [ForeignKey("InstructorCourse")]
        public int CourseId { get; set; }  // ✅ Course Foreign Key

        // ✅ Navigation Properties
        public Student Student { get; set; }
        public InstructorCourse Course { get; set; }
    }
}
