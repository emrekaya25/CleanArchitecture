using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.UserRoles;
using MediatR;

namespace CleanArchitecture.Application.UserRoles;

public sealed record UserRoleDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class UserRoleDeleteCommandHandler(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork) : IRequestHandler<UserRoleDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserRoleDeleteCommand request, CancellationToken cancellationToken)
    {
        UserRole? userRole = await userRoleRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (userRole == null)
            return Result<string>.Failure("Bu kullanıcıya ait atanmış rol bulunamadı");

        userRole.IsDeleted = true;
        userRoleRepository.Update(userRole);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Rol ataması başarıyla gerçekleştirilmiştir.";
    }
}
