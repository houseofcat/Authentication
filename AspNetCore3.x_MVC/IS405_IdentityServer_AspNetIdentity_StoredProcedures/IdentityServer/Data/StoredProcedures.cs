namespace IdentityServer.Data
{
    public static class StoredProcedures
    {
        public static class UserIdentity
        {
            public const string Create = "Identity.UserIdentity_Create";
            public const string Delete = "Identity.UserIdentity_Delete";
            public const string FindByEmail = "Identity.UserIdentity_FindByEmail";
            public const string FindById = "Identity.UserIdentity_FindById";
            public const string FindByName = "Identity.UserIdentity_FindByName";
            public const string Update = "Identity.UserIdentity_Update";
        }

        public static class UserRole
        {
            public const string Create = "Identity.UserRole_Create";
            public const string Delete = "Identity.UserRole_Delete";
            public const string FindById = "Identity.UserRole_FindById";
            public const string FindByName = "Identity.UserRole_FindByName";
            public const string Update = "Identity.UserRole_Update";
        }
    }
}
