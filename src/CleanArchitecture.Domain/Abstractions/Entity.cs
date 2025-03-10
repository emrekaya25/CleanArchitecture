using CleanArchitecture.Domain.AppUsers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace CleanArchitecture.Domain.Abstractions;

//Interface yerine abstract class yapıp kullanıyoruz.
public abstract class Entity : IEntity
{
    public Entity()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }

    #region Audit Log
    public DateTimeOffset CreateAt { get; set; }
    public Guid CreateUserId { get; set; } = default!;
    public DateTimeOffset UpdateAt { get; set; }
    public Guid? UpdateUserId { get; set; }
    public DateTimeOffset DeleteAt { get; set; }
    public Guid? DeleteUserId { get; set; }
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    #endregion

    #region User Infos (Çağrılan kişiyi oluşturan ve güncelleyen kişi bilgilerini bu şekilde alacağız.)
    public string CreateUserName => GetCreateUserName();

    private string GetCreateUserName()
    {
        HttpContextAccessor httpContextAccessor = new();
        var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>(); // httpContext'in oluştuğu esnada uygulamanın bize ServiceRegistiration'da kaydettiği servislere buradan erişiyoruz
        AppUser appUser = userManager.Users.First(x => x.Id == CreateUserId);
        return appUser.FullName + " (" + appUser.Email + ")";
    }

    public string? UpdateUserName => GetUpdateUserName();

    private string? GetUpdateUserName()
    {
        if (UpdateUserId is null) return null;

        HttpContextAccessor httpContextAccessor = new();
        var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>(); // httpContext'in oluştuğu esnada uygulamanın bize ServiceRegistiration'da kaydettiği servislere buradan erişiyoruz
        AppUser appUser = userManager.Users.First(x => x.Id == UpdateUserId);
        return appUser.FullName + " (" + appUser.Email + ")";
    }

    public string? DeleteUserName => GetDeleteUserName();

    private string? GetDeleteUserName()
    {
        if (DeleteUserId is null) return null;

        HttpContextAccessor httpContextAccessor = new();
        var userManager = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<UserManager<AppUser>>();
        AppUser appUser = userManager.Users.First(x => x.Id == DeleteUserId);
        return appUser.FullName + " (" + appUser.Email + ")";
    }
    #endregion
}

