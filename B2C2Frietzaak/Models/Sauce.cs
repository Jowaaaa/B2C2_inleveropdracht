using System.ComponentModel.DataAnnotations;

namespace B2C2Frietzaak.Models
{
    public class Sauce
    {
        [Key]
        public string? SauceId { get; set; }

        public string? SauceName { get; set;}
    }
}
