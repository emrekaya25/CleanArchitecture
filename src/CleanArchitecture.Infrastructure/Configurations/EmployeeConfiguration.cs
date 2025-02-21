using CleanArchitecture.Domain.Employees;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CleanArchitecture.Infrastructure.Configurations;

internal sealed class EmployeeConfiguration : IEntityTypeConfiguration<Employee>
{
    public void Configure(EntityTypeBuilder<Employee> builder)
    {
        builder.OwnsOne(x => x.PersonalInformation, builder =>
        {
            builder.Property(x => x.TcNo).HasColumnName("TCNO");
            builder.Property(x => x.Phone).HasColumnName("Phone");
            builder.Property(x => x.Email).HasColumnName("Email");
        });

        builder.OwnsOne(x => x.Address, builder =>
        {
            builder.Property(x => x.Country).HasColumnName("Country");
            builder.Property(x => x.City).HasColumnName("City");
            builder.Property(x => x.Town).HasColumnName("Town");
            builder.Property(x => x.FullAddress).HasColumnName("Address");
        });

        //builder.Property(x => x.Salary).HasColumnType("money"); ** Decimal tipte olursa bunu yapmazsan hata atar, küsüratı olduğu için. (ya decimal(2,14) ya da money yazılması gerekiyor.)
    }
}

