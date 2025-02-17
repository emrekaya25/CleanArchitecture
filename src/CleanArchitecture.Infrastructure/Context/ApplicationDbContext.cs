using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Users;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure.Context;

internal sealed class ApplicationDbContext : DbContext, IUnitOfWork
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly); // configuration dosyalarının bulunduğu katmanın assembly'sini veriyoruz ve kendisi otomatik buluyor.
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries<Entity>();
        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property(x => x.CreateAt)
                    .CurrentValue = DateTimeOffset.Now;
            }

            if (entry.State == EntityState.Modified)
            {
                if (entry.Property(x => x.IsDeleted).CurrentValue == true)
                {
                    entry.Property(x => x.DeleteAt)
                    .CurrentValue = DateTimeOffset.Now;
                }
                else
                {
                    entry.Property(x => x.UpdateAt)
                    .CurrentValue = DateTimeOffset.Now;
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

