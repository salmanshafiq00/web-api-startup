using System.ComponentModel.DataAnnotations;

namespace WebApiStartup.DTOs
{
    public class RegisterRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        public List<string> Roles { get; set; } = new();
    }
}
