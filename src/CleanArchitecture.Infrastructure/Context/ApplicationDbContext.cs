using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.AppRoles;
using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Employees;
using CleanArchitecture.Domain.UserRoles;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Context;

internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<UserRole> MyUserRoles { get; set; }  
    public DbSet<Employee> Employees { get; set; }
    //Identity kütüphanesi otomatik olarak AppUser'ı dbset olarak ekliyor.

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly); // configuration dosyalarının bulunduğu katmanın assembly'sini veriyoruz ve kendisi otomatik buluyor.

        //IdentityDbContext default olarak 8 tane sınıfı var ve onları tabloya basıyor. İstemediklerimizi bu şekilde ignore etmemiz lazım.
        modelBuilder.Ignore<IdentityUserClaim<Guid>>();
        modelBuilder.Ignore<IdentityRoleClaim<Guid>>();
        modelBuilder.Ignore<IdentityUserToken<Guid>>();
        modelBuilder.Ignore<IdentityUserLogin<Guid>>();
        modelBuilder.Ignore<IdentityUserRole<Guid>>();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        HttpContextAccessor httpContextAccessor = new();
        Guid userId = Guid.Empty; // Default olarak boş GUID kullanıyoruz.

        if (httpContextAccessor.HttpContext != null) // ilk çalıştırmada boş geldiği için hataya düşüyor o yüzden koşul ekledik.
        {
            var userIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(x => x.Type == "user-id");
            if (userIdClaim != null && Guid.TryParse(userIdClaim.Value, out Guid parsedUserId))
            {
                userId = parsedUserId; // giriş yapan kullanıcının id'sini aldık.
            }
        } 

        var entries = ChangeTracker.Entries<IEntity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(x => x.CreateAt)
                    .CurrentValue = DateTimeOffset.Now;
                entry.Property(x => x.CreateUserId)
                    .CurrentValue = userId;
                entry.Property(x => x.IsActive)
                    .CurrentValue = true;
                entry.Property(x => x.IsDeleted)
                    .CurrentValue = false;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(x => x.IsDeleted).CurrentValue == true)
                {
                    entry.Property(x => x.DeleteAt)
                        .CurrentValue = DateTimeOffset.Now;
                    entry.Property(x => x.DeleteUserId)
                        .CurrentValue = userId;
                    entry.Property(x => x.IsActive)
                        .CurrentValue = false;
                }
                else
                {
                    entry.Property(x => x.UpdateAt)
                        .CurrentValue = DateTimeOffset.Now;
                    entry.Property(x => x.UpdateUserId)
                        .CurrentValue = userId;
                    if (entry.Property(x => x.IsActive).CurrentValue == false)
                    {
                        entry.Property(x => x.IsActive)
                        .CurrentValue = true;
                    }
                }
            }

            if (entry.State == EntityState.Deleted)
            {
                throw new ArgumentException("Db'den direkt silme işlemi yapamazsınız.");
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}

