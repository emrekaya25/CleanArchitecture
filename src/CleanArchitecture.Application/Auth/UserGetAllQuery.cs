using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.AppUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;
using System.Data;

namespace CleanArchitecture.Application.Auth;

public sealed record UserGetAllQuery() : IRequest<IQueryable<UserGetAllQueryResponse>>;

public sealed class UserGetAllQueryResponse : EntityDTO
{
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
}

internal sealed class UserGetAllQueryHandler(UserManager<AppUser> userManager) : IRequestHandler<UserGetAllQuery, IQueryable<UserGetAllQueryResponse>>
{
    public Task<IQueryable<UserGetAllQueryResponse>> Handle(UserGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = ( // bu kişi üzerinde işlem yapmış kişileri çekiyoruz aşağıdaki sorgularla
                        from user in userManager.Users.AsQueryable()

                        join create_user in userManager.Users.AsQueryable() on
                        user.CreateUserId equals create_user.Id

                        join update_user in userManager.Users.AsQueryable() on
                        user.UpdateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()

                        join delete_user in userManager.Users.AsQueryable() on
                        user.DeleteUserId equals delete_user.Id into delete_user
                        from delete_users in delete_user.DefaultIfEmpty()

                        select new UserGetAllQueryResponse
                        {
                            Id = user.Id,
                            FirstName = user.FirstName,
                            LastName = user.LastName,

                            CreateAt = user.CreateAt,
                            CreateUserId = user.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",

                            UpdateAt = user.UpdateAt,
                            UpdateUserId = user.UpdateUserId,
                            UpdateUserName = user.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" +
                            update_users.Email + ")",

                            DeleteAt = user.DeleteAt,
                            DeleteUserId = user.DeleteUserId,
                            DeleteUserName = user.DeleteUserId == null ? null : delete_users.FirstName + " " + delete_users.LastName + " (" +
                            delete_users.Email + ")",

                            IsActive = user.IsActive,
                            IsDeleted = user.IsDeleted,
                        }).AsQueryable();

        return Task.FromResult(response);
    }
}
