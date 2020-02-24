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

## Reset Password Flow

This seems to be paticularly tricky to find the latest working example of all in one spot. So I have added additional comments to try and stitch together whats going on top of the 03_IdentityEmailConfirm project.

New simple views.

    [HttpGet("ForgotPassword")]
    public IActionResult ForgotPassword() => View();

    [HttpGet("ForgotPasswordEmailSent")]
    public IActionResult ForgotPasswordEmailSent() => View();

    [HttpGet("ResetPasswordConfirmed")]
    public IActionResult ResetPasswordConfirmed() => View();

The Post from the ForgotPassword view. This creates the token, the url, and sends out the email.

    [HttpPost("ForgotPassword")]
    public async Task<IActionResult> ForgotPasswordAsync(string email)
    {
        if (email == null) { return RedirectToAction("ForgotPassword"); }

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null) { return BadRequest(); }

        // Generate Password Token
        var passwordToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        passwordToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(passwordToken));

        // Generate Password Reset Url
        var passwordResetUrl = Url
            .Action(
                nameof(ResetPassword),
                "Home",
                new { userId = user.Id, passwordToken },
                Request.Scheme,
                Request.Host.ToString());

        // Generate an HTML Link from Verification URL
        var passwordResetHtml = $"<a href='{HtmlEncoder.Default.Encode(passwordResetUrl)}'>Click here to reset your password!</a>";

        // Send email through EmailService (MailKit NetCore)
        await _emailService.SendAsync("test@email.com", "Password Reset Request", passwordResetHtml, true);

        return RedirectToAction("ForgotPasswordEmailSent");
    }

New ViewModel view that allows user to submit a new password. This View/Url is the one the Email gives takes the user to.

    [HttpGet("ResetPassword")]
    public IActionResult ResetPassword(string userId, string passwordToken)
    {
        if (userId == null || passwordToken == null) { return RedirectToAction("Index"); }

        passwordToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(passwordToken));

        return View(new ResetPassword { UserId = userId, Token = passwordToken }); // Passing in the ViewModel.
    }

ResetPasswordAsync is the post form ResetPassword, allows you to validate model and reset the password based on the password reset token.

    [HttpPost("ResetPassword")]
    public async Task<IActionResult> ResetPasswordAsync(ResetPassword resetPassword) // Receiving the ViewModel from post.
    {
        if (!ModelState.IsValid) { return View(resetPassword); }

        if (resetPassword.UserId == null || resetPassword.Token == null || resetPassword.NewPassword == null)
        { return RedirectToAction("Index"); }

        var user = await _userManager.FindByIdAsync(resetPassword.UserId);
        if (user == null) { return BadRequest(); }

        var result = await _userManager.ResetPasswordAsync(user, resetPassword.Token, resetPassword.NewPassword);
        if (!result.Succeeded) { return BadRequest(); }

        return RedirectToAction("ResetPasswordConfirmed");
    }
