﻿using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Data.Configuration
{
    internal sealed class RoleConfiguration : IEntityTypeConfiguration<IdentityRole>
    {
        public void Configure(EntityTypeBuilder<IdentityRole> builder)
        {
            IdentityRole[] identityRoles =
            {
                new()
                {
                    Name = "User",
                    NormalizedName = "USER"
                },
                new()
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN",
                }
            };


            builder.HasData(identityRoles);
        }
    }
}
