﻿using CleanArchitecture.Application.Services;
using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace CleanArchitecture.Application.Auth;

public sealed record LoginCommand(
    string UserNameOrEmail,
    string Password) : IRequest<Result<LoginCommandResponse>>;

public sealed record LoginCommandResponse
{
    public string AccessToken { get; set; } = default!;
    public string FullName { get; set; } = default!;
}

internal sealed class LoginCommandHandler(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
    IJwtProvider jwtProvier) : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
{
    public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        AppUser? user = await userManager.Users.FirstOrDefaultAsync(x => x.Email == request.UserNameOrEmail || x.UserName == request.UserNameOrEmail);

        if (user is null)
        {
            return Result<LoginCommandResponse>.Failure("Kullanıcı bulunamadı");
        }

        SignInResult signInResult = await signInManager.CheckPasswordSignInAsync(user, request.Password, true);

        if (signInResult.IsLockedOut)
        {
            TimeSpan? timeSpan = user.LockoutEnd - DateTimeOffset.UtcNow;
            if (timeSpan is not null)
            {
                return (500, $"Şifrenizi 3 defa yanlış girdiğiniz için kullanıcı {Math.Ceiling(timeSpan.Value.TotalMinutes)} dakika süreyle bloke edilmiştir.");
            }
            else
            {
                return (500, "Kullanıcınız 3 kez yanlış şifre girdiği için 5 dakika süreyle bloke edilmiştir.");
            }
        }

        if (signInResult.IsNotAllowed)
        {
            return (500,"Mail adresiniz onaylı değil");
        }

        if (!signInResult.Succeeded)
        {
            return (500,"Şifreniz yanlış");
        }

        //token üretme

        var token = await jwtProvier.CreateTokenAsync(user,cancellationToken);

        var response = new LoginCommandResponse()
        {
            AccessToken = token,
            FullName = user.FullName
        };

        return response;
    }
}
