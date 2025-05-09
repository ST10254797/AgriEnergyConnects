using System.ComponentModel.DataAnnotations;

namespace AgriEnergyConnects.Models
{
    public class Farmer
    {
        public int Id { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Location")]
        public string Location { get; set; }
    }
}
