using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Users;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Infrastructure.Context;

internal sealed class ApplicationDbContext : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Employees { get; set; }

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
        string userIdString = httpContextAccessor.HttpContext!.User.Claims.First(x => x.Type == "user-id").Value;
        Guid userId = Guid.Parse(userIdString); // giriş yapan kullanıcının id'sini aldık.

        var entries = ChangeTracker.Entries<Entity>();
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

