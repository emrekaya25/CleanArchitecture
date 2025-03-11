using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.UserRoles;
using FluentValidation;
using Mapster;
using MediatR;

namespace CleanArchitecture.Application.UserRoles;

public sealed record UserRoleCreateCommand(
    Guid userId,
    Guid roleId) : IRequest<Result<string>>;

public sealed class UserRoleCreateCommandValidator : AbstractValidator<UserRoleCreateCommand>
{
    public UserRoleCreateCommandValidator()
    {
        RuleFor(x => x.userId).NotEmpty().WithMessage("Kullanıcı bilgisi boş bırakılamaz");
        RuleFor(x => x.roleId).NotEmpty().WithMessage("Rol bilgisi boş bırakılamaz");
    }
}

internal sealed class UserRoleCreateCommandHandler(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork) : IRequestHandler<UserRoleCreateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserRoleCreateCommand request, CancellationToken cancellationToken)
    {
        var isUserRoleExist = await userRoleRepository.AnyAsync(x=>x.UserId == request.userId && x.RoleId == request.roleId);
        if (isUserRoleExist)
            return Result<string>.Failure("Bu rol bu kullanıcı için daha önce atanmış");

        UserRole userRole = new UserRole()
        {
            UserId = request.userId,
            RoleId = request.roleId,
        };
        userRoleRepository.Add(userRole);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Başarıyla eklendi";
    }
}
