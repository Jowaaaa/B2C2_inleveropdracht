using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2C2Frietzaak.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        [ForeignKey("Product")]
        public int ProductId { get; set; }
        public string? ProductName { get; set; }
        public float? Price { get; set; }
        public string? ImageUrl { get; set; }
        public int Quantity { get; set; }
        public float? Total
        {
            get { return Quantity * Price; }
        }


        // Navigatie Properties
        public Cart? Cart { get; set; }
        public Product? Product { get; set; }



    }
}
