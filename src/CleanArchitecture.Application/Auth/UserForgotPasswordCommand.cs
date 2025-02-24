using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Auth;

// Bu ikisi yenileme token oluşturmak için.
public sealed record UserForgotPasswordCommand(
    string email) : IRequest<Result<string>>;

internal sealed class UserForgotPasswordCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<UserForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByEmailAsync(request.email);

        if (appUser is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }

        string token = await userManager.GeneratePasswordResetTokenAsync(appUser);

        return Result<string>.Succeed(token);
    }
}

public sealed record UserChangeForgotPasswordCommand(
    string email,
    string newPassword,
    string token):IRequest<Result<string>>;

internal sealed class UserChangeForgotPasswordCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<UserChangeForgotPasswordCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserChangeForgotPasswordCommand request, CancellationToken cancellationToken)
    {
        AppUser? appUser = await userManager.FindByEmailAsync(request.email);

        if (appUser is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }

        IdentityResult result = await userManager.ResetPasswordAsync(appUser,request.token,request.newPassword);

        if (!result.Succeeded)
        {
            return Result<string>.Failure("Şifre değiştirilemedi, şifrenizi kontrol ediniz.");
        }

        return Result<string>.Succeed($"{appUser.FullName} adlı kullanıcı için şifre başarıyla değiştirildi.");
    }
}

