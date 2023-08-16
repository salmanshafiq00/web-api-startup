using System.ComponentModel.DataAnnotations;

namespace WebApiStartup.DTOs
{
    public record UpdateCategoryDTO
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
