# Authentication - IS403 IdentityServerAspNetIdentity

## Demonstrates

 * All of IS402_IdentityServerMvcClient
 * Wiring up of Serilog in Program.cs to a database.
 * Dependency inject IConfiguration using a scope in Main.
 * Serilog console sink.
 * Serilog configuration based wire-up (SqlServer).
 * Installing EntityFramework Core Tools (or updating them)
 * How to extend Configurations to cleanup Startup.cs

## Note
THIS project will start using a Database. Sql Server 2019. Which you can download and use for development purposes for free.

Download
1.) https://www.microsoft.com/en-us/sql-server/sql-server-downloads
2.) Click `Download 2019 Developer edition`
3.) I have included a generic new Database Window screenshot. Super simple setup out of the box. Make sure your account is owner.
    * Alternatively you could run a Database build script that may or may not work for you. These things break or stop working all the time when changing versions.

## List of Dependencies For Serilog, IdentityServer4, EF, AspNetCoreIdentity

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

## EntityFramework Tools
You need to have this installed on your machine.  
https://docs.microsoft.com/en-us/ef/core/miscellaneous/cli/dotnet  

Also added package to this project (dotnet add package Microsoft.EntityFrameworkCore.Design)  

### Install

    dotnet tool install --global dotnet-ef  

### Update

    dotnet tool update --global dotnet-ef  

### Update (specific version)  

    dotnet tool update --global dotnet-ef --version 3.1.2  


### Build Out Your Migrations

    dotnet ef migrations add AppDbContextMigration -c IS403_IdentityServer.Data.AppDbContext -o Data/Migrations/ApplicationDb
    dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb
    dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb

## Sources

Identity Tutorial - Raw Coding Youtube Guide (This is just taught really well)  
https://www.youtube.com/watch?v=0GAnnCA3u_I
