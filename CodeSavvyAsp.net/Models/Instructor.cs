using System.ComponentModel.DataAnnotations;

namespace CodeSavvyAsp.net.Models
{
    public class Instructor
    {
        [Key]
        public int Id { get; set; }

        [Required]

        public string FirstName { get; set; }

        [Required]
         
        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }


        [Required]
        [Phone(ErrorMessage ="Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Password { get; set; }



    }
}
