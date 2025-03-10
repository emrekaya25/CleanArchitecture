using CleanArchitecture.Domain.AppRoles;
using CleanArchitecture.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Roles;

public sealed record RoleGetQuery(
    Guid Id) : IRequest<Result<AppRole>>;


internal sealed class RoleGetQueryHandler(RoleManager<AppRole> roleManager) : IRequestHandler<RoleGetQuery, Result<AppRole>>
{
    public async Task<Result<AppRole>> Handle(RoleGetQuery request, CancellationToken cancellationToken)
    {
        AppRole? role = await roleManager.FindByIdAsync(request.Id.ToString());

        if (role is null)
           return Result<AppRole>.Failure("Rol bulunamadı");

        return role;
    }
}
