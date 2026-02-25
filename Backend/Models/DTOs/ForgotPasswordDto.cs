using System.ComponentModel.DataAnnotations;

namespace Models.DTOs;

public class ForgotPasswordDto
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;
}

public class ResetPasswordDto
{
    public string Token { get; set; } = null!;
    public string NewPassword { get; set; } = null!;
}