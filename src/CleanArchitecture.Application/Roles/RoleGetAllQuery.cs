using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.AppRoles;
using CleanArchitecture.Domain.AppUsers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.Roles;

public sealed record RoleGetAllQuery() : IRequest<IQueryable<RoleGetAllQueryResponse>>;

public sealed class RoleGetAllQueryResponse : EntityDTO
{
    public string? Name { get; set; }
}

internal sealed class RoleGetAllQueryHandler(RoleManager<AppRole> roleManager,UserManager<AppUser> userManager) : IRequestHandler<RoleGetAllQuery, IQueryable<RoleGetAllQueryResponse>>
{
    public Task<IQueryable<RoleGetAllQueryResponse>> Handle(RoleGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (
                        from role in roleManager.Roles.AsQueryable()

                        join create_user in userManager.Users.AsQueryable() on
                        role.CreateUserId equals create_user.Id
                        //update
                        join update_user in userManager.Users.AsQueryable() on
                        role.UpdateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()
                        //delete
                        join delete_user in userManager.Users.AsQueryable() on
                        role.DeleteUserId equals delete_user.Id into delete_user
                        from delete_users in delete_user.DefaultIfEmpty()

                        select new RoleGetAllQueryResponse
                        {
                            Id = role.Id,
                            Name = role.Name,

                            CreateAt = role.CreateAt,
                            CreateUserId = role.CreateUserId,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",

                            UpdateAt = role.UpdateAt,
                            UpdateUserId = role.UpdateUserId,
                            UpdateUserName = role.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" +
                            update_users.Email + ")",

                            DeleteAt = role.DeleteAt,
                            DeleteUserId = role.DeleteUserId,
                            DeleteUserName = role.DeleteUserId == null ? null : delete_users.FirstName + " " + delete_users.LastName + " (" +
                            delete_users.Email + ")",

                            IsActive = role.IsActive,
                            IsDeleted = role.IsDeleted
                        }).AsQueryable();

        return Task.FromResult(response);
    }
}