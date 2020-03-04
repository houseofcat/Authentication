
# May need to execute manually - gave me lame excuse and work just fine individually. Have to be in IdentityServer project root.

dotnet ef migrations remove -c IdentityServer.Data.AppDbContext
dotnet ef migrations remove -c PersistedGrantDbContext
dotnet ef migrations remove -c ConfigurationDbContext