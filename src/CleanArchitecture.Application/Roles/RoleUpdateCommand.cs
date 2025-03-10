using CleanArchitecture.Domain.AppRoles;
using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using FluentValidation;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Roles;

public sealed record RoleUpdateCommand(
    Guid Id,
    string Name):IRequest<Result<string>>;

public sealed class RoleUpdateCommandValidator : AbstractValidator<RoleUpdateCommand>
{
    public RoleUpdateCommandValidator()
    {
        RuleFor(x => x.Name).MinimumLength(2).WithMessage("Rol ismi 2 karakterden küçük olamaz");
    }
}

internal sealed class RoleUpdateCommandHandler(RoleManager<AppRole> roleManager, IUnitOfWork unitOfWork) : IRequestHandler<RoleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleUpdateCommand request, CancellationToken cancellationToken)
    {
        AppRole? role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role is null)
            return Result<string>.Failure("Güncellenecek rol bulunamadı");

        request.Adapt(role);
        await roleManager.UpdateAsync(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rol başarıyla güncellendi";
    }
}
