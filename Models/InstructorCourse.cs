using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CodeSavvyAsp.Models
{
    public class InstructorCourse
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Course Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [StringLength(1000, ErrorMessage = "Description cannot exceed 1000 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [StringLength(50, ErrorMessage = "Duration cannot exceed 50 characters")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        [Precision(10, 2)]
        public decimal Price { get; set; }

        // Link to Instructor
        public int InstructorId { get; set; }
        public Instructor Instructor { get; set; }
    }
}
