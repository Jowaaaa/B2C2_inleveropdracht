namespace B2C2Frietzaak.Models.ViewModels
{
    public class ProductsViewModel
    {
        public List<Product>? Products { get; set; }
        public List<CartItem>? CartItems { get; set; }
        public List<Cart>? Carts { get; set; }
        public List<GroupedProduct>? GroupedProducts { get; set; }
        public List<Sauce>? Sauces { get; set; }
        public List<Order>? Orders { get; set;}

    }
}

