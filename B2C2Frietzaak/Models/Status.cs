namespace B2C2Frietzaak.Models
{
    public class Status
    {
        public int StatusId { get; set; }
        public string? StatusName { get; set; }

        //Navigation properties:
        public List<Order>? Orders { get; set; }
    }
}
