using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels
{
    public class RegisterViewModel
    {
        [PersonalData, Required]
        public string UserName { get; set; }

        [PersonalData, Required, DataType(DataType.Password)]
        public string Password { get; set; }

        [PersonalData, Required, DataType(DataType.Password), Compare("Password")]
        public string ConfirmPassword { get; set; }

        [PersonalData, Required, DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        public bool RememberMe { get; set; }

        public string ReturnUrl { get; set; }
    }
}
