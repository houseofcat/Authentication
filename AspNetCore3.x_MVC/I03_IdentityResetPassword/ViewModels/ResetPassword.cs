using System.ComponentModel.DataAnnotations;

namespace _04_IdentityResetPassword.ViewModels
{
    public class ResetPassword
    {
        [Required]
        public string UserId { get; set; }

        [Required]
        public string Token { get; set; }

        [Required]
        [StringLength(30)]
        public string NewPassword { get; set; }
    }
}
