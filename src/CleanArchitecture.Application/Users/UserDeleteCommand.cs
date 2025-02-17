using CleanArchitecture.Domain.Common.Repositories;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users;

public sealed record UserDeleteCommand(
    Guid Id) : IRequest<Result<string>>;

internal sealed class UserDeleteCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<UserDeleteCommand, Result<string>>
{
    public async Task<Result<string>> Handle(UserDeleteCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.FirstOrDefaultAsync(x => x.Id == request.Id,cancellationToken);
        if (user is null)
        {
            return Result<string>.Failure("Kullanıcı bulunamadı.");
        }
        user.IsDeleted = true;
        userRepository.Update(user);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return $"{user.FullName} isimli kullanıcı başarıyla silindi.";
    }
}

