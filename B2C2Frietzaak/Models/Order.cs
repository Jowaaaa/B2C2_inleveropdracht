using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace B2C2Frietzaak.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public int CartId { get; set; }
        [ForeignKey("UserId")]
        [Required]
        public string? UserId { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser? User { get; set; }
        [Required]
        public DateTime? OrderTime { get; set; } = DateTime.Now;
        public List<Cart>? Cart { get; set; }

    }
}
