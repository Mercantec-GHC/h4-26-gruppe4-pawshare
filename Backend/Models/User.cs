using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class User : Common
    {
        [Required(ErrorMessage = "Brugernavn er påkrævet.")]
        [Display(Name = "Navn")]
        public required string Name { get; set; }
        public int RoleId { get; set; }
        public Role Role { get; set; } = null!;

        [Required(ErrorMessage = "Email er påkrævet.")]
        [EmailAddress(ErrorMessage = "Email er ikke gyldig")]
        [Display(Name = "Email")]
        public required string Email { get; set; }
        public required string City { get; set; }
        
        [Required(ErrorMessage = "Adgangskode er påkrævet.")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$", 
            ErrorMessage = "Der skal være mindst 8 karaktere, mindst et stort bogstav, et lillebogstav og et tal")]
        [Display(Name = "Adgangskode")]
        public required string HashedPassword { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiresAt { get; set; }
        public string? PasswordResetToken { get; set; }
        public DateTime? PasswordResetTokenExpiresAt { get; set; }
        public string? ProfilePictureKey { get; set;  }
        public List<Animal>? Animals { get; set; }
        public List<UserAppointmentBooking>? Bookings { get; set; }
        public List<ChatUserConvo>? Chats { get; set; }
        public List<MessageReadReceipt>? readMessages { get; set; }
    }
}
