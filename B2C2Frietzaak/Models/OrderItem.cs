namespace B2C2Frietzaak.Models
{
    public class OrderItem
    {
        public int OrderItemId { get; set; }
        public int Quantity { get; set; }

        //Foreign key en navigation properties
        public int OrderId { get; set; }
        public Order? Order { get; set; }

        public int ProductId { get; set; }
        public Product? Product { get; set; }

        public int? SauceId { get; set; }
        public Sauce? Sauce { get; set; }
    }
}
