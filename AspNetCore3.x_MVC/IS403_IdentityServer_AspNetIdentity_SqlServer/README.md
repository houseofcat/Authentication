# Authentication - IS403 IdentityServer_AspNetIdentity_SqlServer

## Demonstrates

 * All of IS402_IdentityServerMvcClient
 * Wiring up of Serilog in Program.cs to a database.
   * Use IConfiguration (via DI in a scope) in Main.
   * Serilog console sink.
   * Serilog configuration based wire-up (SqlServer).
 * Installing EntityFramework Core Tools (or updating them)
 * How to extend IServiceCollection to cleanup Startup.cs
 * Wiring up AspNetIdentity To SqlServer
 * Wiring up IdentityServer4 To SqlServer
 * Generating EF Core Migrations.
 * Creating a method on Startup (If Debug) to try and build out Database.

## SqlServer
This project will start using a Database but I will not demonstrate the steps to install it well.  

Database Version: Sql Server 2019 Developer Edition.  
The Developer edition is free for development purposes.  
  
1.) Link: https://www.microsoft.com/en-us/sql-server/sql-server-downloads  
2.) Click `Download 2019 Developer edition`  
3.) I have included a simple generic new Database Window screenshot - just make sure your account is owner.  
***Alternatively, you could run the included Database build script that may or may not work for you. These things break or stop working all the time when changing versions.***  

The included CreateDatabase scripts are based on what you see in `ConnectionStrings` in `appSetttings.json`.  
I took screenshots to give you an idea of what it generates.

## List of Dependencies For Serilog, IdentityServer4, EF, AspNetIdentity
Please review these new dependencies and proceed when you feel comfortable with what all is what. Review Startup.cs/ConfigureExtensions.cs  

    <ItemGroup>
      <PackageReference Include="IdentityServer4" Version="3.1.2" />
      <PackageReference Include="IdentityServer4.AspNetIdentity" Version="3.1.2" />
      <PackageReference Include="IdentityServer4.EntityFramework" Version="3.1.2" />
      <PackageReference Include="Microsoft.AspNetCore.Identity.EntityFrameworkCore" Version="3.1.2" />
      <PackageReference Include="Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation" Version="3.1.2" />
      <PackageReference Include="Microsoft.EntityFrameworkCore.Design" Version="3.1.2">
        <PrivateAssets>all</PrivateAssets>
        <IncludeAssets>runtime; build; native; contentfiles; analyzers; buildtransitive</IncludeAssets>
      </PackageReference>
      <PackageReference Include="Microsoft.EntityFrameworkCore.SqlServer" Version="3.1.2" />
      <PackageReference Include="Serilog" Version="2.9.0" />
      <PackageReference Include="Serilog.AspNetCore" Version="3.2.0" />
      <PackageReference Include="Serilog.Settings.Configuration" Version="3.1.0" />
      <PackageReference Include="Serilog.Sinks.MSSqlServer" Version="5.1.3" />
    </ItemGroup>

**AspNetIdentity is inside NetCoreApp3.1, do not include the 2.x.x Nuget packages.**

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
This will take the two classes (that come from IdentityServer library) and your own AppDbContext to structure out the code version of how your Database should look.  

```powershell
                                                              # Full Name Space To AppDbContext
dotnet ef migrations add Initial_AppDbContextMigration -c IdentityServer.Data.AppDbContext -o Data/Migrations/ApplicationDb  
dotnet ef migrations add Initial_IS4_PersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb  
dotnet ef migrations add Initial_IS4_ConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb  
```

On Startup (if Debug) will actually execute the creation of this Database plan. That is migrations in a nutshell.  
The history (of whether a migration has been ran/executed) is stored in the dbo.__efMigrationsHistory. If a migration  
has already been ran, it won't run again thanks to this table. To get it to actually run again you will have to delete  
the entries of this table (and potentially all the other objects/tables/procedures it created).  

### NetCore SDK 3.1.2
https://dotnet.microsoft.com/download/dotnet-core/3.1  

## Sources

Microsoft Doc - Entity Framework Core tools reference - .NET CLI
https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet  

...and just me really.