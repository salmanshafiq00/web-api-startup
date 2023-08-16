using System.ComponentModel.DataAnnotations;

namespace WebApiStartup.DTOs
{
    public class LoginRequestDTO
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
