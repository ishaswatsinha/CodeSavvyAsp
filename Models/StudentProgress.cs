using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSavvyAsp.Models
{
    public class StudentProgress
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]  // ✅ Auto-increment enabled
        public int Id { get; set; }

        [Required]
        public int StudentId { get; set; }

        [Required]
        public int CourseId { get; set; }

        public bool IsCompleted { get; set; } = false;  // ✅ Default is "Not Completed"

        public DateTime? CompletedOn { get; set; }  // ✅ Stores completion date

        // Optional: Navigation property (if needed)
        [ForeignKey("StudentId")]
        public virtual Student Student { get; set; }

        [ForeignKey("CourseId")]
        public virtual InstructorCourse Course { get; set; }
    }
}
