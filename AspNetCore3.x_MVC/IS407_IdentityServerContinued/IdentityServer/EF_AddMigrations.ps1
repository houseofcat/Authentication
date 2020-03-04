
# May need to execute manually - gave me lame excuse and work just fine individually. Have to be in IdentityServer project root.

dotnet ef migrations add AppDbContextMigration -c IdentityServer.Data.AppDbContext -o Data/Migrations/ApplicationDb
dotnet ef migrations add InitialIdentityServerPersistedGrantDbMigration -c PersistedGrantDbContext -o Data/Migrations/PersistedGrantDb
dotnet ef migrations add InitialIdentityServerConfigurationDbMigration -c ConfigurationDbContext -o Data/Migrations/ConfigurationDb