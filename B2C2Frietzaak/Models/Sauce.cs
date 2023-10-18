using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace B2C2Frietzaak.Models
{
    public class Sauce
    {
        public int SauceId { get; set; }
        [Display(Name = "Saus")]
        [Required]
        public string? SauceName { get; set;}
    }
}
