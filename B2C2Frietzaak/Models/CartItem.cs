using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2C2Frietzaak.Models
{
    public class CartItem
    {
        [Key]
        public int CartItemId { get; set; }
        
        public int Quantity { get; set; }

        public List<Product>? Products { get; set; }

        // Navigatie Properties
        public Product? Product { get; set; }
        public int CartId { get; set; }
        public Cart? Cart { get; set; }
    }
}
