using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Display(Name = "Afbeelding")]
        public string? ImageUrl { get; set; }

        // foreign key en navigation properties:
        [ForeignKey("CategoryId")]
        [Display(Name="Categorie")]
        public int CategoryId {  get; set; }
        public Category? Category { get; set; }

        public int? SauceId { get; set; }
        public Sauce? Sauce { get; set;}

        public List<OrderItem>? OrderItems { get; set; }

        [NotMapped]
        [Display(Name = "Image")]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }
    }
}
