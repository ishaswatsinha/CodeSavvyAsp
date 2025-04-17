namespace CodeSavvyAsp.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema; // ✅ Added for Foreign Keys

public class EnrolledCourse
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Student")]
    public int StudentId { get; set; }  // ✅ Reference to Student Table

    [Required]
    [ForeignKey("Course")]
    public int CourseId { get; set; }   // ✅ Reference to Course Table

    // ✅ Navigation Properties (For Relations)
    public Student? Student { get; set; }
    public Course Course { get; set; }
}
