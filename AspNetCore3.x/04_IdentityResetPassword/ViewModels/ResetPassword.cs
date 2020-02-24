namespace _04_IdentityResetPassword.ViewModels
{
    public class ResetPassword
    {
        public string UserId { get; set; }
        public string Token { get; set; }
        public string NewPassword { get; set; }
    }
}
