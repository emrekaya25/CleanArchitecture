using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Users;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Users;
public sealed record UserGetQuery(
    Guid Id) : IRequest<Result<User>>;

internal sealed class UserGetQueryHandler(
    IUserRepository userRepository) : IRequestHandler<UserGetQuery, Result<User>>
{
    public async Task<Result<User>> Handle(UserGetQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (user is null)
        {
            return Result<User>.Failure("Kullanıcı bulunamadı");
        }

        return user;
    }
}
