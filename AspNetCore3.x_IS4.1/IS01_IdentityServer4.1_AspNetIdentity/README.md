# Authentication - IS01_IdentityServer4.1_AspNetIdentity

## Demonstrates

 * Setting up Serilog via extension method from my other library.
 * Appsettings.json for Serilog.
 * AspNetIdentity setup via extension method.
 * IdentityServer4 setup for SqlServer via extension method.
 
## Basic Get Started/Setup
Just remember the setup is not for production, the names of Databases, Schemas, and Tables are of my choosing.  
That means name them what you want - or better yet - involve a DBA. I generally separate out my `Identity`  
stuff away from regular database activity. I also generally having a logging database.

 1.) Install SqlServer 2019 (if needed).  
 2.) Install SMSS 2018 (if needed).  
 3.) Create `Identity` database from wizard or script (if wanted).  
 4.) First run we will have executed the DB migrations included in this project.  
 5.) Verify the migration created tables under `Identity` database.  

### SqlServer 2019
This project will start using a Database but **I will not fully detail the steps to install it well.**  
Under `DBSetup_Server` folder, I did demonstrate a quick and dirty way to do a quick local install.  

Database Version: Sql Server 2019 Developer Edition.  
The Developer edition is free for development purposes.  
  
Link: https://www.microsoft.com/en-us/sql-server/sql-server-downloads  

Click `Download 2019 Developer edition`  

### SqlServer 2019 Management  
Today there are a couple of tools you could use, I am going to stick with the main (but also include a cross-platform option).   

You should get familiar with browsing and exploring your local databases anyways as this will prepare you for working with production databases located elsewhere.  

#### Sql Server Management Studio 2018 (SSMS 2018)
[Microsoft Doc](https://docs.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver15)

**Inside SSMS 2018**  
I have included a simple generic new Database Window screenshot - just make sure your account is owner of the database.  
***Alternatively, you could run the included Database build script that may or may not work for you. These things break or stop working all the time when changing versions.***  

The included CreateDatabase scripts are based on what you see in `ConnectionStrings` in `appSetttings.json`.  
I took screenshots to give you an idea of what it generates after they have ran.

#### Azure Data Studio (formerly SQL Operations Studio)  
[Microsoft Doc](https://docs.microsoft.com/en-us/sql/azure-data-studio/download-azure-data-studio?view=sql-server-ver15)   

### List of Dependencies For Serilog, IdentityServer4.1, EF, AspNetIdentity
Located in the CSPROJ.


### Serilog For MS SqlServer
[More Serilog SqlServer Details](https://github.com/serilog/serilog-sinks-mssqlserver)

## EntityFramework Tools
You need to have this installed on your machine for Entity Framework migrations.  
https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet  

Also added package to this project (dotnet add package Microsoft.EntityFrameworkCore.Design)  

### Install

```powershell
dotnet tool install --global dotnet-ef
```

### Update

```powershell
dotnet tool update --global dotnet-ef
```

### Update (specific version)  

```powershell
dotnet tool update --global dotnet-ef --version 3.1.2  
```

### Build Out Your Migrations
This will take the two classes (that come from IdentityServer library) and your own AppDbContext to  
structure out the code version of how your Database should look.  

```powershell
                                                              # Full Name Space To AppDbContext
dotnet ef migrations add Initial_AppDbContextMigration -c IdentityServer.Data.AppDbContext -o Data/Migrations/ApplicationDb  
dotnet ef migrations add Initial_IS4_PersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb  
dotnet ef migrations add Initial_IS4_ConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb  
```

On Startup (if Debug) will actually execute the creation of this Database plan. That is how migrations work in a nutshell.  
The history (of whether a migration has been ran/executed) is stored in the dbo.__EFMigrationsHistory. If a migration  
has already been ran, it won't run again thanks to this table. To get it to actually run again you will have to delete  
the entries from this table (and potentially all the other objects/tables/procedures it created to prevent errors). Alternatively,
you make adjustments then add additional migrations which will get executed the next time you Startup in Debug.

### NetCore SDK 3.1.9 / Net 5.0
https://dotnet.microsoft.com/download/dotnet-core/3.1  

## Sources

IdentityServer4 Doc - Logging  
https://identityserver4.readthedocs.io/en/latest/topics/logging.html  

Microsoft Doc - Entity Framework Core tools reference - .NET CLI
https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet  

Microsoft Doc - EF Migrations
https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli

Microsoft Doc - Configure ASP.NET Core Identity
https://docs.microsoft.com/en-us/aspnet/core/security/authentication/identity-configuration?view=aspnetcore-3.1

...and just from my own noggin.