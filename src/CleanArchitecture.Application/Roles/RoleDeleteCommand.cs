using CleanArchitecture.Domain.AppRoles;
using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Roles;

public sealed record RoleDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class RoleDeleteCommandHandler(RoleManager<AppRole> roleManager,IUnitOfWork unitOfWork) : IRequestHandler<RoleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(RoleDeleteCommand request, CancellationToken cancellationToken)
    {
        AppRole? role = await roleManager.FindByIdAsync(request.Id.ToString());
        if (role == null)
            return Result<string>.Failure("Rol bulunamadı");

        role.IsDeleted = true;
        await roleManager.UpdateAsync(role);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return $"{role.Name} isimli rol başarıyla silindi";
    }
}
