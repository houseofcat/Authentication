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

I recommend purging the last set of migrations and running the new migrations checked in. To do that you  
generally have to remove the entries from the `dbo.__EFMigrationsHistory` table and to delete all the  
`Identity` tables. You can leave `IdentityLogging` alone or purge the old entries. To help drop tables,  
there is a script in the sql scripts from last time.   

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
Pretty much anything in EntityFrameworkCore can be replaced with your own signature style. In this  
particular project, we are replacing two stores that use AspNetIdentity leverages EF.Core to run  
queries on users and role. Instead, we would rather execute our custom written stored procedures.  

The reasons one may do this vary...  

It could be because you don't like Entity Framework.  
Maybe you prefer to use Dapper with StoredProcedures.  
Maybe you are trying to re-use older Users and Schemas that already exist in different objects/tables.  
Maybe your DBA forces you to use StoredProcedures/Tables that are tweaked to their specification.  
Maybe you just want the ability to update StoredProcedures on the fly to fix bugs?  
The reason isn't really the problem, it's the `how do I do this?` that's the problem.  

We don't have any functioning features yet like create user or login pages... so I will revisit it to  
make sure its working later on.  

## UserStore / RoleStore
Both are using `storedprocedures` instead of EF Core. It accomplishes this with some helper classes.  

DataSource (a wrapping class) for DapperHelper.  
DapperHelper, which in turn, is a wrapper for Dapper.  
Completely overkill but I had this code already written though from a gist so I used it.  

I also demonstrate FastMember for quick object property to parameter mapping.  

## Stored Procedures

List of generalized stored procedures for you to use under the StoredProcedures folder.  

These stored procedures could be named whatever you want but you will have to update the names  
in code.  

```
Identity.UserIdentity_Create.sql
Identity.UserIdentity_Delete.sql
Identity.UserIdentity_FindByEmail.sql
Identity.UserIdentity_FindById.sql
Identity.UserIdentity_FindByName.sql
Identity.UserIdentity_Update.sql

Identity.UserRole_Create.sql
Identity.UserRole_Delete.sql
Identity.UserRole_FindById.sql
Identity.UserRole_FindByName.sql
Identity.UserRole_Update.sql
```

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