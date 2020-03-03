# Authentication - IS406 IdentityServer

## Demonstrates

 * Create users via AccountController (ApiController).  
   * Just needs a Post request to create users.  
 * Automapper and Mapping Profiles.
   * Mapping a CreateUserRequest object to an IdentityUser object.  
 * Carrying on the login flow of IS402.  
   * Added a cleaned up TestMvcClient project (now using Serilog.)  
     * The settings are found `appsettings.json` and the table is `[IdentityLogging].[TestMvcClient]`  
 * Custom Middleware to better integrate with AspNetCore Request logging.  
 * Modifications to Serilog Logging table to add more columns and JSON (instead of XML).
 * Replacing JsonFormatter with the Utf8Json one (nuget `Utf8Json.AspNetCoreMvcFormatter`).  
 * Adding compression to ResponseBody.

## Taking A Step Back
Things escalated pretty quick with all the customizations that went on, but now I want to bring the  
focus back to Authentication/Authorization.  

To do that, we are keeping Sql Server, Serilog, and other enhancements. We are removing all the custom  
replacement classes and switching back to the built-ins.  

It's best to purge all the tables and the previous migrations before proceeding - all of the tools needed  
are in the previous few examples and their `README.md`.  

## Additional Help
I have included a PowerShell script to create a TestUser or you can use the Postman collection provided.  

## Note on Creating Classes That Inherit UserManager<TUser>

The definition UserManager<IdentityUser> and SignInManager<IdentityUser>  
require a Scope resolve from Dependency Injection.  

If you were to go with custom classes akin to Services that have  
UserManager (et cetera) as dependencies, the service can only be added  
properly with AddScoped. If your service was designed as a singleton,  
this could effect performance significantly. You may need to find a work  
around in such a case (i.e. using custom AspNet classes.).  

Example.) Service being added with AddSingleton will cause an InvalidOperationException. 

    services.AddSingleton<IUserService>(s =>
    {
        return new UserService(
            s.GetRequiredService<UserManager<IdentityUser>>(),
            s.GetRequiredService<SignInManager<IdentityUser>>());
    });

So we will be using UserManager and SignInManager directly into the
Controllers themselves.  

## Serilog
Porbably best to delete all IdentityLogging tables before starting up. I have learned on how to get things  
more desireable.

## Caution: Response Compression with HTTPS
Compressed responses over secure connections can be controlled with the EnableForHttps option, which is  
`disabled by default`. Using compression with dynamically generated pages can lead to security problems  
such as the CRIME and BREACH attacks.  

## Sources

IdentityServer4 - Authorize Endpoint  
https://identityserver4.readthedocs.io/en/latest/endpoints/authorize.html  

Microsoft Doc - SignInManager<TUser> Class  
https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1?view=aspnetcore-3.1  

Microsoft Doc - SignInManager<TUser>.PasswordSignInAsync  
https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.signinmanager-1.passwordsigninasync?view=aspnetcore-3.1  

Microsoft Doc - Invoke-RestMethod  
https://docs.microsoft.com/en-us/powershell/module/Microsoft.PowerShell.Utility/Invoke-RestMethod?view=powershell-5.1  

NetCore 3.0 - Replace .EnableRewind() with .EnableBuffering() aka Re-reading Http Request bodies.  
https://devblogs.microsoft.com/aspnet/re-reading-asp-net-core-request-bodies-with-enablebuffering/  

Microsoft Doc - ArrayPool  
https://docs.microsoft.com/en-us/dotnet/api/system.buffers.arraypool-1?view=netstandard-2.1  

Microsoft Doc - Asp.Net Core Middleware  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1  

Microsoft Doc - Write custom ASP.NET Core middleware  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/write?view=aspnetcore-3.1  

Github Serilog - Serilog.Sinks.MSSqlServer #CustomPropertyColumns  
https://github.com/serilog/serilog-sinks-mssqlserver#custom-property-columns  

Microsoft Doc - Response compression in ASP.NET Core  
https://docs.microsoft.com/en-us/aspnet/core/performance/response-compression?view=aspnetcore-3.1  

Microsoft Doc - Response caching in ASP.NET Core  
https://docs.microsoft.com/en-us/aspnet/core/performance/caching/memory?view=aspnetcore-3.1  