namespace B2C2Frietzaak.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }
        public int ProductId { get; set; }
        public int Quantity { get; set; }

        // Navigatie Properties
        public Cart? Cart { get; set; }
        public Product? Product { get; set; }
    }
}
