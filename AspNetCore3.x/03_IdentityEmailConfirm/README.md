# Authentication - 03 IdentityEmailConfirm

## Demonstrates

 * All of 02_BasicIdentity
 * Alternative method for cleaner Get View in HomeController.
 * Startup.cs constructor dependency injecting configuration.
   * Appsettings.json is automatically loaded as IConfiguration.
 * Demonstrate AppSettings Configuration Section reading.
 * Configure a new Email Smtp Service (MailKit)
 * Require Email Confirmations
   * Found in .AddIdentity<TUser, TRole>(IdentityOptions => {...})
 * Register user no longer redirects to Index.
   * Generates a email verification token to be used in an email.

 ## Sources

 SmtpServer for Test Applications
 https://github.com/ChangemakerStudios/Papercut

 Microsoft Docs - Configuration  
https://docs.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-3.1  

 Microsoft Docs - SmtpClient Deprecated  
https://docs.microsoft.com/en-us/dotnet/api/system.net.mail.smtpclient?view=netframework-4.8  
Recommends MailKit https://github.com/jstedfast/MailKit  
This is the NetCore version https://github.com/myloveCc/NETCore.MailKit  
 
Identity Tutorial - Raw Coding Youtube Guide (This is just taught really well)  
https://www.youtube.com/watch?v=Vj7iCb7wDs0