using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.UserRoles;
using MediatR;

namespace CleanArchitecture.Application.UserRoles;

public sealed record UserRoleGetQuery(
    Guid Id) : IRequest<Result<UserRole>>;

internal sealed class UserRoleGetQueryHandler(IUserRoleRepository userRoleRepository) : IRequestHandler<UserRoleGetQuery, Result<UserRole>>
{
    public async Task<Result<UserRole>> Handle(UserRoleGetQuery request, CancellationToken cancellationToken)
    {
        UserRole? userRole = await userRoleRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (userRole is null)
            return Result<UserRole>.Failure("Kullanıcı ile eşleşen rol bulunamadı");

        return userRole;
    }
}
