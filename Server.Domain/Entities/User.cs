using System.ComponentModel.DataAnnotations.Schema;
using Server.Domain.Enums;

namespace Server.Domain.Entities
{
    public class User : BaseEntity
    {
        public int RoleId { get; set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public double? Balance { get; set; }
        public string? RefreshToken { get; set; }
        public StatusEnum? Status { get; set; }
        public string? Otp { get; set; }
        public bool IsVerified { get; set; } = false;
        public DateTime? OtpExpiryTime { get; set; }
        public string? VerificationToken { get; set; }
        public string? ResetToken { get; set; }
        public DateTime? ResetTokenExpiry { get; set; }
        public bool? IsStaff { get; set; }

        [ForeignKey("RoleId")]
        public Role Role { get; set; }
    }
}
