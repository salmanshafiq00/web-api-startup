using System.ComponentModel.DataAnnotations;

namespace WebApiStartup.DTOs
{
    public class CreateProductDTO
    {
        [Required]
        public string Name { get; set; }
        public string? Description { get; set; }
        [Required]
        public decimal Price { get; set; }
        public DateTime? ExpireDate { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
