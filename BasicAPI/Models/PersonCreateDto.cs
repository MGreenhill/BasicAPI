using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace BasicAPI.Models
{
    public class PersonCreateDto
    {
        //DTO for Post Requests
        //Only accepts FirstName, LastName, Profession, and Age.
        [Required(ErrorMessage ="")]
        [StringLength(25)]
        public string FirstName { get; set; }

        [Required(ErrorMessage ="")]
        [StringLength(25)]
        public string LastName { get; set; }

        [StringLength(50)]
        public string Profession { get; set; }

        [Range(0,110)]
        public int Age { get; set; }
    }
}
