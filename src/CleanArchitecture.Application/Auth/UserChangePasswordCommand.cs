using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Auth;

public sealed record UserChangePasswordCommand(
    Guid userId,
    string currentPassword,
    string newPassword) : IRequest<Result<string>>;

internal sealed class UserChangePasswordCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<UserChangePasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var AppUser = await userManager.FindByIdAsync(request.userId.ToString());

        if (AppUser is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }

        IdentityResult result = await userManager.ChangePasswordAsync(AppUser,request.currentPassword,request.newPassword);

        if (!result.Succeeded)
        {
            return Result<string>.Failure("Şifre değiştirilemedi, şifrenizi kontrol ediniz.");
        }

        return Result<string>.Succeed($"{AppUser.FullName} adlı kullanıcı için şifre başarıyla değiştirildi.");
    }
}

