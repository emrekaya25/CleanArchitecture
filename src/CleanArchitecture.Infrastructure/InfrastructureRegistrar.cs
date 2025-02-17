using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Context;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Infrastructure;

public static class InfrastructureRegistrar
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(opt =>
        {
            string connectionString = configuration.GetConnectionString("SqlServer") ??
            throw new Exception("Sql bağlantısı kurulamadı.");
            opt.UseSqlServer(connectionString);
        });

        services.AddScoped<IUnitOfWork>(srv => srv.GetRequiredService<ApplicationDbContext>());

        //services.AddScoped<IUserRepository, UserRepository>(); 
        //yerine alttaki scrutor ile scan metodunu kullanıyoruz.

        services.Scan(opt =>
            opt.FromAssemblies(typeof(InfrastructureRegistrar).Assembly)
            .AddClasses(publicOnly: false)
            .UsingRegistrationStrategy(RegistrationStrategy.Skip)
            .AsImplementedInterfaces()
            .WithScopedLifetime()
        );
        // UsingRegistrationStrategy(RegistrationStrategy.Skip) -> eğer daha önceden DI yapılmış varsa onu geç yapma 2.defa
        //WithScopedLifeTime() ile bitiriyoruz.
        return services;
    }
}

