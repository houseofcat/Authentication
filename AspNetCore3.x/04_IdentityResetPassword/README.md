# Authentication - 04 IdentityResetPassword

## Demonstrates

 * All of 03_IdentityEmailConfirm
 * Added ResetPasswordRequest and ResetPassword.
 * Modified how ResetPassword and EmailConfirmation tokens are sent.
   * Included corrections found at GitHub and Stackoverflow and Microsoft's Identity scaffolding templates.
 * Converting the dependency injected members to private readonly variables (as is convention.)

## Sources
Code Generation Problems  
https://github.com/dotnet/aspnetcore/issues/8325
https://stackoverflow.com/questions/38781295/asp-net-core-identity-invalid-token-on-confirmation-email
https://github.com/dotnet/aspnetcore/issues/5789

Identity Scaffolding Examples With Correction

    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));

    var callbackUrl = Url.Page(
        "/Account/ConfirmEmail",
        pageHandler: null,
        values: new { area = "Identity", userId = user.Id, code = code },
        protocol: Request.Scheme);

    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

and then reading it back when received

    code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));