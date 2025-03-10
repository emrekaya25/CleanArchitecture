using CleanArchitecture.Domain.AppRoles;
using CleanArchitecture.Domain.Common.Results;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Roles;

public sealed record RoleCreateCommand(string Name) : IRequest<Result<string>>;

public sealed class RoleCreateCommandValidator : AbstractValidator<RoleCreateCommand>
{
    public RoleCreateCommandValidator()
    {
        RuleFor(x => x.Name).MinimumLength(2).WithMessage("Rol adı en az 2 karakter olmalıdır.");
    }
}

internal sealed class RoleCreateCommandHandler(RoleManager<AppRole> roleManager) : IRequestHandler<RoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleCreateCommand request, CancellationToken cancellationToken)
    {
        var isRoleExist = roleManager.Roles.Any(x => x.Name == request.Name); // normalizedName'e göre kontrol edilecek.
        if (isRoleExist)
        {
            return Result<string>.Failure("Rol için bu isim kullanılmış");
        }

        AppRole role = new()
        {
            Name = request.Name,
        };

        IdentityResult identityResult = await roleManager.CreateAsync(role);

        if (!identityResult.Succeeded)
        {
            return Result<string>.Failure("Rol eklenirken bir hata oluştu");
        }

        return Result<string>.Succeed("Rol başarıyla eklendi");
    }
}

