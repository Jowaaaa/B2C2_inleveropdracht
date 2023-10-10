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
        [Required]
        public string? UserId { get; set; }
        public IdentityUser? User { get; set; }
        [Required]
        public DateTime? OrderTime { get; set; } = DateTime.Now;
        [DataType(DataType.Currency)]
        public float? Total { get; set; }

        public List<OrderItem>? OrderItems { get; set; }

    }
}
