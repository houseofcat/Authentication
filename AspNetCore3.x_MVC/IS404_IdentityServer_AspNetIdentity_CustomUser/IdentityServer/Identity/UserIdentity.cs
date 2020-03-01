using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Identity
{
    // IdentityUser defaults Id (primary key) to type string. Here we demonstrate how to replace that with long.
    // More happens in AppDbContext OnModelBuild
    public class UserIdentity : IdentityUser<long> // long is being placed into TKey value.
    {
        // Hypothetically speaking, let's also rename the primary key. First off you have to add the property that
        // will take over as the new "Id" but with the T-Sql data type of long - bigint.
        [Column(TypeName = "bigint")] // specify T-Sql Data type
        public long UserId { get; set; }

        // IdentityUser property Email is being overriden to replace nvarchar with varchar and a size of 200.
        // This isn't necessarily a good idea - just demonstrating how to change IdentityUser generated properties
        // in the migration.
        [Column(TypeName = "varchar(200)")] // specify T-Sql Data type
        public override string Email { get; set; }

        // IdentityUser doesn't have this property so we are adding it.
        [Column(TypeName = "int")] // specify T-Sql Data type
        public int RegionId { get; set; }

        // Additional property we want to add.
        [Column(TypeName = "int")] // specify T-Sql Data type
        public int CurrentRegionId { get; set; }

        // Additional property we want to add.
        [Column(TypeName = "datetime2(0)")] // specify T-Sql Data type and precision
        public DateTime CreatedDatetime { get; set; }

        // Additional property we want to add.
        [Column(TypeName = "datetime2(0)")] // specify T-Sql Data type and precision
        public DateTime ModifiedDatetime { get; set; }

        // Additional property we want to add.
        [Column(TypeName = "bit")] // specify T-Sql Data type
        public bool IsActive { get; set; }
    }
}
