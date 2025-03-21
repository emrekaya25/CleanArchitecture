﻿using CleanArchitecture.Application.Behaviors;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Application;

public static class ApplicationRegistrar
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddMediatR(configure =>
            {
                configure.RegisterServicesFromAssembly(typeof(ApplicationRegistrar).Assembly); //Bu katmanın assembly'si verilmesi gerekiyor.
                configure.AddOpenBehavior(typeof(ValidationBehavior<,>));
            });

            services.AddValidatorsFromAssembly(typeof(ApplicationRegistrar).Assembly);// ValidationBehavior'un içindeki IValidator'e erişebilmesi için bu yazılıyor.

            return services;
        }
    }

