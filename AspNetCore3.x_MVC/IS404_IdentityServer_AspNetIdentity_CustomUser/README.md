# Authentication - IS404 IdentityServer_AspNetIdentity_CustomUser

## Demonstrates

 * All of IS403_IdentityServerMvcClient  
 * Sql Server (Transact-SQL) Data Types  
 * Creating a custom IdentityUser  
 * Creating a custom IdentityRole  
 * Demonstrating how to add properties.  
 * Demonstrating how to change IdentityUser primary key name and the data type.  
 * Demonstrating how to use different tables etc.  
 * Demonstrating how to specify specific DB datatypes instead of what automatically gets picked for DB.  
 * Modifying the properties of some objects.  
 * Deleting all old migrations to make way for the new migrations.  
 * Adding Serilog to the actual Kestrel host and to also capture requests too!  

## Requires Everything Setup From IS403
That is, in order to work out of the box.  
This project is based on having EF.Core & Tools already setup.  
That you have taken care of SqlServer (or written your own connection strings and data persistence layer).  

I recommend purging the last set of migrations and running the new migrations checked in.

## The General Theme With AspNetIdentity
Everything is customizeable... ...but it's not all cleanly labeled in a single spot or demonstrated.  

To see whats changed try and focus on the comments. Keep in mind really important stuff is generally left in. So its not a law but a good rule of thumb is that a comment
pertains to whats being changed for this project.

## Sources

IdentityServer4 Doc - Logging  
https://identityserver4.readthedocs.io/en/latest/topics/logging.html  

Microsoft Doc - Data types (Transact-Sql)  
https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver15  

Microsoft Doc - Identity model customization in ASP.NET Core  
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1  

Microsoft Doc - Add, download, and delete custom user data to Identity in an ASP.NET Core project  
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/add-user-data?view=aspnetcore-3.1&tabs=visual-studio  