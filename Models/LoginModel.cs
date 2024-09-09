namespace proyectoCanchas.Models;
using System.ComponentModel.DataAnnotations;
public class LoginModel
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
     [MinLength(5, ErrorMessage = "Password must be at least 5 characters long")]
    [DataType(DataType.Password)]
    public string Password { get; set; }
}