using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;
public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}