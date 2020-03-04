using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace IdentityServer.ViewModels
{
    public class LoginViewModel
    {
        [PersonalData, Required]
        public string UserName { get; set; }
        [PersonalData, Required, DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public string ReturnUrl { get; set; }
    }
}
