# Authentication - IS406 IdentityServer

## Demonstrates

 * Create users via AccountController (ApiController).  
   * Just needs a Post request to create users.  
 * Automapper and Mapping Profiles.
   * Mapping a CreateUserRequest object to an IdentityUser object.  
 * Carrying on the login flow of IS402.  
   * Added a cleaned up TestMvcClient project (now using Serilog.)  
     * The settings are found `appsettings.json` and the table is `[IdentityLogging].[TestMvcClient]`  

## Taking A Step Back
Things escalated pretty quick with all the customizations that went on, but now I want to bring the  
focus back to Authentication/Authorization.  

To do that, we are keeping Sql Server, Serilog, and other enhancements. We are removing all the custom  
replacement classes and switching back to the built-ins.  

It's best to purge all the tables and the previous migrations before proceeding - all of the tools needed  
are in the previous few examples and their `README.md`.  

## Additional Help
I have included a PowerShell script to create a TestUser or you can use the Postman collection provided.  

## Sources

IdentityServer4 - Authorize Endpoint  
https://identityserver4.readthedocs.io/en/latest/endpoints/authorize.html  

Microsoft Doc - Invoke-RestMethod
https://docs.microsoft.com/en-us/powershell/module/Microsoft.PowerShell.Utility/Invoke-RestMethod?view=powershell-5.1  