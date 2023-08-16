using System.ComponentModel.DataAnnotations;

namespace WebApiStartup.DTOs
{
    public record CreateCategoryDTO
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string? PhotoUrl { get; set; }
    }
}
