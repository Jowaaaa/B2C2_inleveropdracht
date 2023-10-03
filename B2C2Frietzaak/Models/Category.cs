namespace B2C2Frietzaak.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string? Name { get; set; }

        //Navigation properties:
        public List<Product>? Products { get; set; }
    }
}
