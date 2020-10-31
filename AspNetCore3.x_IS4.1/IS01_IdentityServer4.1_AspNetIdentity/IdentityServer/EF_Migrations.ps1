# Update EF Tools
dotnet tool update --global dotnet-ef

# Build Migrations
dotnet ef migrations add Users -c IdentityServer.Data.ApplicationDbContext -o Data/Migrations/IdentityServer/ApplicationDb
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/IdentityServer/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/IdentityServer/ConfigurationDb