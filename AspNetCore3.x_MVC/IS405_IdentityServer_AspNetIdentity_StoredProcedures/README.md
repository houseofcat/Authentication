# Authentication - IS405 IdentityServer_AspNetIdentity_StoredProcedures

## Demonstrates

 * All of IS404_IdentityServer_AspNetIdentity_CustomUser  
 * Simplifying CreateSerilogLogger() in `Main`.
 * Custom UserStore and Interface/function implementations.
 * Custom RoleStore and Interface/function implementations.
 * Added Stored Procedures.
 * Dapper with wrappings and FastMember usage.
 * An abstraction layer of DataSource.

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

## Replacing UserStore / RoleStore
Pretty much anything in EntityFrameworkCore can be replaced with your own signature style. In this particular project, we are replacing  
two key components that use EF.Core to run queries against users and role lookups. We would rather execute our custom stored procedures for instead.  

The reason one may do this varies. It could be because you don't like Entity Framework. Maybe you prefer to use Dapper with StoredProcedures?  
Maybe your DBA forces you to use StoredProcedures/Tables that are tweaked to their specification.  
Maybe you just want the ability to update StoredProcedures on the fly to fix bugs?  
The reason isn't really the problem, it's the `how do I do this?` that's the problem.  

## UserStore<TUser> || UserStore<UserIdentity> (our custom one)


## Sources

Microsoft Doc - Custom storage providers for ASP.NET Core Identity  
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-custom-storage-providers?view=aspnetcore-3.1  

Microsoft Doc - UserStore<TUser> Class  
https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore.userstore-1?view=aspnetcore-3.0  

Microsoft Github Source - UserStore  
https://github.com/dotnet/aspnetcore/blob/master/src/Identity/EntityFrameworkCore/src/UserStore.cs  

Microsoft Doc - RoleStore<TRole> Class  
https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore.rolestore-1?view=aspnetcore-3.0  

Microsoft Github Source - RoleStore  
https://github.com/dotnet/aspnetcore/blob/master/src/Identity/EntityFrameworkCore/src/RoleStore.cs  

Microsoft Doc - Microsoft.AspNetCore.Identity.EntityFrameworkCore Namespace
https://docs.microsoft.com/en-us/dotnet/api/microsoft.aspnetcore.identity.entityframeworkcore?view=aspnetcore-3.0