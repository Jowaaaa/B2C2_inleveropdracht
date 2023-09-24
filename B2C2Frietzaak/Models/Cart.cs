using System.ComponentModel.DataAnnotations;

namespace B2C2Frietzaak.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        public string? UserId { get; set;}
        public List<CartItem>? CartItems { get; set; }
    }
}
