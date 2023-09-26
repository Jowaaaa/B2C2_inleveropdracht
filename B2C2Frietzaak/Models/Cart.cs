using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace B2C2Frietzaak.Models
{
    public class Cart
    {
        [Key]
        public int CartId { get; set; }
        [ForeignKey("UserId")]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
        public List<CartItem>? CartItems { get; set; }

    }
}
