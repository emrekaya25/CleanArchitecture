﻿using CleanArchitecture.Domain.AppUsers;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.WebAPI;
public static class ExtensionsMiddleware
{
    public static void CreateFirstUser(WebApplication app)
    {
        using (var scoped = app.Services.CreateScope())
        {
            var userManager = scoped.ServiceProvider.GetRequiredService<UserManager<AppUser>>();

            if (!userManager.Users.Any(x => x.UserName == "admin"))
            {
                AppUser user = new()
                {
                    UserName = "admin",
                    Email = "admin@admin.com",
                    FirstName = "Emre",
                    LastName = "Kaya",
                    EmailConfirmed = true,
                    CreateAt = DateTimeOffset.Now,
                };

                user.CreateUserId = user.Id;

                userManager.CreateAsync(user,"1").Wait();
            }
        }
    }
}

