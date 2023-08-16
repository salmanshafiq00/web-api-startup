namespace WebApiStartup.Entity
{
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public string? PhotoUrl { get; set; }

        public List<Product> Products { get; set; } = new List<Product>();
    }
}
