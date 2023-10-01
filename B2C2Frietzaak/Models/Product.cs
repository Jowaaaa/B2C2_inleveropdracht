using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace B2C2Frietzaak.Models
{
    public class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Required]
        [Display(Name ="Naam")]
        public string? Name { get; set; }
        [Required]
        [DataType(DataType.Currency)]
        [Display(Name = "Prijs")]
        public float? Price { get; set; }
        [Required]
        [Display(Name = "Afbeelding")]
        public string? ImageUrl { get; set; }
        public string? Category {  get; set; }

    }
}
