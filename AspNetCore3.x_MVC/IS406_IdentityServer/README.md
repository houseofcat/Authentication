# Authentication - IS406 IdentityServer

## Demonstrates

 * Create users via AccountController (ApiController).
   * Just needs a Post request to create users.
 * Automapper and Mapping Profiles mapping a CreateUserRequest to an IdentityUser.

## Taking A Step Back
Things escalated pretty quick with all the customizations that went on, but now I want to bring the  
focus back to Authentication/Authorization.  

To do that, we are keeping Sql Server, Serilog, and other enhancements. We are removing all the custom  
replacement classes and switching back to the built-ins.  

## Sources

Microsoft Doc - Invoke-RestMethod
https://docs.microsoft.com/en-us/powershell/module/Microsoft.PowerShell.Utility/Invoke-RestMethod?view=powershell-5.1  