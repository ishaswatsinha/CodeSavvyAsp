using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CodeSavvyAsp.Models
{
    public class InstructorCourse
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters.")]
        public string Title { get; set; }

        [Required]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters.")]
        public string Description { get; set; }

        [Required]
        [StringLength(50, ErrorMessage = "Duration cannot exceed 50 characters.")]
        public string Duration { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Price must be a positive value.")]
        public decimal Price { get; set; }

      

        // Adding InstructorId for database mapping
        [ForeignKey("Instructor")]
        public int InstructorId { get; set; }

        // Navigation property for the related Instructor
        public Instructor? Instructor { get; set; } // 👈 make it nullable

        public string? ImageUrl { get; set; } // Store image path or URL instead 


    }
}
