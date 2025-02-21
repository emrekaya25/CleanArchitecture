using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.Common.Results;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanArchitecture.Application.Auth;
public sealed record UserGetQuery(
    Guid Id) : IRequest<Result<AppUser>>;

internal sealed class UserGetQueryHandler(UserManager<AppUser> userManager) : IRequestHandler<UserGetQuery, Result<AppUser>>
{
    public async Task<Result<AppUser>> Handle(UserGetQuery request, CancellationToken cancellationToken)
    {
        var user = await userManager.Users.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (user is null)
        {
            return Result<AppUser>.Failure("Kullanıcı bulunamadı");
        }

        return user;
    }
}

