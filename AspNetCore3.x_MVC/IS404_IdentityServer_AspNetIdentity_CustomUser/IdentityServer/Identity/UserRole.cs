using Microsoft.AspNetCore.Identity;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace IdentityServer.Identity
{
    // Again, we are changing the inherited primary key Id from string
    public class UserRole : IdentityRole<long>
    {
        [Column(TypeName = "bigint")]
        public override long Id { get; set; }

        // Additional property we want to add.
        [Column(TypeName = "datetime2(0)")]
        public DateTime CreatedDatetime { get; set; }

        // Additional property we want to add.
        [Column(TypeName = "datetime2(0)")]
        public DateTime ModifiedDatetime { get; set; }
    }
}
