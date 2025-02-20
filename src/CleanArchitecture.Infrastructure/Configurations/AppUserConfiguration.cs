using CleanArchitecture.Domain.AppUsers;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Configurations;
internal sealed class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> builder)
    {
        builder.HasIndex(x => x.UserName).IsUnique(); //UserName artık unique oldu.

        builder.Property(x => x.FirstName).HasColumnType("varchar(50)");
        builder.Property(x => x.LastName).HasColumnType("varchar(50)");
        builder.Property(x => x.UserName).HasColumnType("varchar(15)");
        builder.Property(x => x.Email).HasColumnType("varchar(MAX)");
    }
}
