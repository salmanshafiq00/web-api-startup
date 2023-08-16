namespace WebApiStartup.Entity
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public DateTime? ExpireDate { get; set; }
        public int CategoryId { get; set; }
        public string? PhotoUrl { get; set; }

        // navagational property
        public Category Category { get; set; } = new();
    }
}
