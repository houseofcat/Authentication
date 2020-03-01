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

I recommend purging the last set of migrations and running the new migrations checked in. To do that you generally have to remove  
the entries from the `dbo.__EFMigrationsHistory` table and to delete all the `Identity` tables. You can leave `IdentityLogging` alone  
or purge the old entries. To help drop tables, there is a script in the sql scripts from last time.   

My philosophy has always been `nuke the site from orbit, just to be sure.`  

```tsql
------------------------------------
------------------------------------

-- Creating a bunch of DROP table scripts as the output
-- Based on the Schema 'Identity' stored in @Schema
USE [Identity]

DECLARE @SqlStatement NVARCHAR(MAX)
DECLARE @Schema VARCHAR(32) = 'Identity'

SELECT @SqlStatement = 
    COALESCE(@SqlStatement, N'') + N'DROP TABLE IF EXISTS [' + @Schema +'].' + QUOTENAME(TABLE_NAME) + N';' + CHAR(13)
FROM INFORMATION_SCHEMA.TABLES
WHERE TABLE_SCHEMA = @Schema and TABLE_TYPE = 'BASE TABLE'

PRINT @SqlStatement

------------------------------------
------------------------------------
```

## The General Theme With AspNetIdentity
Everything is customizeable... ...but it's not all cleanly labeled in a single spot or demonstrated.   

 * Data\AppDbContext.cs || modified
 * Identity\UserIdentity.cs || new
 * Identity\UserRole.cs || new
 * StartupExtensions.cs || modified
 * Data\Migrations\* || all freshly minted
 * Program.cs || webhost enhanced with logging.
 * Startup.cs || App builder added Serilog request logging middleware.

To see what's changed, try and focus on the `comments`. Keep in mind really important items, like warnings, are generally left in as well.  

Once everything has been set, the migrations have made, old stuff deleted, and you startup successfully with the new colorful console, you can view your tables and verify  
changes have been made such as table name, propery name for primary key, or new fields have been added.  

A `verification.PNG` was added to show what to look for.  

## Sources

IdentityServer4 Doc - Logging  
https://identityserver4.readthedocs.io/en/latest/topics/logging.html  

Microsoft Doc - Data types (Transact-Sql)  
https://docs.microsoft.com/en-us/sql/t-sql/data-types/data-types-transact-sql?view=sql-server-ver15  

Microsoft Doc - Identity model customization in ASP.NET Core  
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/customize-identity-model?view=aspnetcore-3.1  

Microsoft Doc - Add, download, and delete custom user data to Identity in an ASP.NET Core project  
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/add-user-data?view=aspnetcore-3.1&tabs=visual-studio  

Microsoft Doc - IdentityDbContext<TUser,TRole,TKey> Class
https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore.identitydbcontext-3?view=aspnetcore-3.0  