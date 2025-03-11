using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.UserRoles;
using FluentValidation;
using Mapster;
using MediatR;

namespace CleanArchitecture.Application.UserRoles;

public sealed record UserRoleUpdateCommand(
    Guid Id,
    Guid UserId,
    Guid RoleId) : IRequest<Result<string>>;

public class UserRoleUpdateCommandValidator : AbstractValidator<UserRoleUpdateCommand>
{
    public UserRoleUpdateCommandValidator()
    {
        RuleFor(x => x.UserId).NotNull().WithMessage("Kullanıcı bilgisi boş bırakılamaz");
        RuleFor(x => x.RoleId).NotNull().WithMessage("Rol bilgisi boş bırakılamaz");
    }
}

internal sealed class UserRoleUpdateCommandHandler(IUserRoleRepository userRoleRepository, IUnitOfWork unitOfWork) : IRequestHandler<UserRoleUpdateCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserRoleUpdateCommand request, CancellationToken cancellationToken)
    {
        UserRole? userRole = await userRoleRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (userRole is null)
            return Result<string>.Failure("Kullanıcı ile eşleşen rol bulunamadı.");

        request.Adapt(userRole);
        userRoleRepository.Update(userRole);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return "Başarıyla Güncellendi";
    }
}
