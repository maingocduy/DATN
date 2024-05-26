using System.ComponentModel.DataAnnotations;

namespace WebApplication3.DTOs.Auth
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Username Required")]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password Required")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
