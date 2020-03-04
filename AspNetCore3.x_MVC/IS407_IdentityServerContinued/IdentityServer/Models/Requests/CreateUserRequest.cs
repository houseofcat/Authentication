using System.ComponentModel.DataAnnotations;

namespace IdentityServer.Requests
{
    public class CreateUserRequest
    {
        [MaxLength(200)]
        [MinLength(6)]
        [Required]
        public string Email { get; set; }

        [MaxLength(200)]
        [MinLength(8)]
        [Required]
        public string Password { get; set; }

        public string UserName { get; set; }
    }
}
