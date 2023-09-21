using System.ComponentModel.DataAnnotations;

namespace BasicAPI.Models
{
    public class PersonUpdateDto
    {
        //DTO for Put requests
        [Required]
        public int Id { get; set; }

        [Required(ErrorMessage = "Please provide a first name.")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="Please provide a last name.")]
        [StringLength(25)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Profession { get; set; }

        [Range(0, 110, ErrorMessage = "Age is outside the acceptable range.")]
        public int Age { get; set; }
    }
}
