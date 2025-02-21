using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Common.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Auth;

public sealed record UserCreateCommand(
    string FirstName,
    string LastName,
    string UserName,
    string Email,
    string Password
    ) : IRequest<Result<string>>;

public sealed class UserCreateCommandValidator : AbstractValidator<UserCreateCommand>
{
    public UserCreateCommandValidator()
    {
        RuleFor(x => x.FirstName).MinimumLength(3).WithMessage("Ad alanı en az 3 karakter olmalıdır.")
                                .NotEmpty().WithMessage("Ad alanı boş olamaz");
        RuleFor(x => x.LastName).MinimumLength(3).WithMessage("Soyad alanı en az 3 karakter olmalıdır.")
                                .NotEmpty().WithMessage("Soyad alanı boş olamaz");
        RuleFor(x => x.UserName).MinimumLength(3).WithMessage("Kullanıcı adı alanı en az 3 karakter olmalıdır.")
                                .NotEmpty().WithMessage("Kullanıcı adı alanı boş olamaz");
        RuleFor(s => s.Email).NotEmpty().WithMessage("Email adres alanı boş olamaz.")
                     .EmailAddress().WithMessage("Geçerli bir email giriniz");
    }
}

internal sealed class UserCreateCommandHandler(UserManager<AppUser> userManager) : IRequestHandler<UserCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserCreateCommand request, CancellationToken cancellationToken)
    {
        var isUserExist = userManager.Users.Any(x => x.UserName == request.UserName || x.Email == request.Email);

        if (isUserExist)
        {
            return Result<string>.Failure("Kullanıcı adı veya emailinizi kontrol ediniz!");
        }

        AppUser user = new()
        {
            UserName = request.UserName,
            Email = request.Email,
            FirstName = request.FirstName,
            EmailConfirmed = true,
            LastName = request.LastName,
            CreateAt = DateTimeOffset.Now,
        };

        user.CreateUserId = user.Id;//ekleyen kişi accessor ile eklenecek.

        IdentityResult identityResult = await userManager.CreateAsync(user,request.Password);

        if (!identityResult.Succeeded)
        {
            return Result<string>.Failure("Kullanıcı eklenirken bir hata oluştu.");
        }

        return Result<string>.Succeed("Kullanıcı başarıyla eklendi");
    }
}

