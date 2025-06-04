using System.ComponentModel.DataAnnotations;

namespace APIverse.Models
{
    public class DigiPinRequest
    {
        [Required]
        [Range(2.5, 38.5, ErrorMessage = "Latitude must be between 2.5 and 38.5")]
        public double Latitude { get; set; }

        [Required]
        [Range(63.5, 99.5, ErrorMessage = "Longitude must be between 63.5 and 99.5")]
        public double Longitude { get; set; }
    }
}
