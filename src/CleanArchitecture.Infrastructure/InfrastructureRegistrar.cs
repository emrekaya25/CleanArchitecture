using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Users;
using CleanArchitecture.Infrastructure.Context;
using CleanArchitecture.Infrastructure.Options;
using CleanArchitecture.Infrastructure.Repositories;
using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
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


        //bu bağlantıyı userManager'ı kullanabilmek için yapıyoruz(UserManager'ın DI'ı).
        services
            .AddIdentity<AppUser, IdentityRole<Guid>>(opt =>
            {
                opt.Password.RequiredLength = 1;
                opt.Password.RequireNonAlphanumeric = false;
                opt.Password.RequireDigit = false;
                opt.Password.RequireLowercase = false;
                opt.Password.RequireUppercase = false;
                opt.Lockout.MaxFailedAccessAttempts = 5;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                opt.SignIn.RequireConfirmedEmail = true;
            }) //Identity kütüphanesini tanımlar
            .AddEntityFrameworkStores<ApplicationDbContext>() // ApplicationDbContextle bağlantısını ayarlıyor
            .AddDefaultTokenProviders(); // UserManager'ın token üretme methodunu çalışabilmesiin sağlıyor.


        //services.AddScoped<IUserRepository, UserRepository>(); 
        //yerine alttaki scrutor ile scan metodunu kullanıyoruz.

        //JWTOptions tanımlama
        services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
        services.ConfigureOptions<JwtOptionsSetup>();

        services.Scan(opt => opt
                .FromAssemblies(typeof(InfrastructureRegistrar).Assembly)
                .AddClasses(publicOnly: false)
                .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                .AsImplementedInterfaces()
                .WithScopedLifetime()
                );
        // UsingRegistrationStrategy(RegistrationStrategy.Skip) -> eğer daha önceden DI yapılmış varsa onu geç yapma 2.defa
        //WithScopedLifeTime() ile bitiriyoruz.

        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer();
        services.AddAuthorization();

        var config = TypeAdapterConfig.GlobalSettings;
        config.Scan(typeof(ApplicationDbContext).Assembly); // Burada Mapster configlerini tarıyoruz!
        services.AddSingleton(config);

        return services;
    }
}

