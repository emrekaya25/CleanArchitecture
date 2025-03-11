using CleanArchitecture.Domain.Abstractions;
using CleanArchitecture.Domain.AppRoles;
using CleanArchitecture.Domain.AppUsers;
using CleanArchitecture.Domain.UserRoles;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace CleanArchitecture.Application.UserRoles;

public sealed record UserRoleGetAllQuery() : IRequest<IQueryable<UserRoleGetAllQueryResponse>>;

public sealed class UserRoleGetAllQueryResponse : EntityDTO
{
    public string? UserName { get; set; }
    public string? RoleName { get; set; }
}

internal sealed class UserRoleGetAllQueryHandler(IUserRoleRepository userRoleRepository, UserManager<AppUser> userManager,RoleManager<AppRole> roleManager) : IRequestHandler<UserRoleGetAllQuery, IQueryable<UserRoleGetAllQueryResponse>>
{
    public Task<IQueryable<UserRoleGetAllQueryResponse>> Handle(UserRoleGetAllQuery request, CancellationToken cancellationToken)
    {
        var response = (
                        from userRole in userRoleRepository.GetAll()

                        join create_user in userManager.Users.AsQueryable() on
                        userRole.CreateUserId equals create_user.Id

                        join update_user in userManager.Users.AsQueryable() on
                        userRole.UpdateUserId equals update_user.Id into update_user
                        from update_users in update_user.DefaultIfEmpty()

                        join delete_user in userManager.Users.AsQueryable() on
                        userRole.DeleteUserId equals delete_user.Id into delete_user
                        from delete_users in delete_user.DefaultIfEmpty()

                        //userName alanı doldurma
                        join userName in userManager.Users.AsQueryable() on 
                        userRole.UserId equals userName.Id
                        //roleName alanı doldurma
                        join roleName in roleManager.Roles.AsQueryable() on
                        userRole.RoleId equals roleName.Id

                        select new UserRoleGetAllQueryResponse
                        {
                            Id = userRole.Id,
                            UserName = userName.FullName,
                            RoleName = roleName.Name,

                            CreateAt = userRole.CreateAt,
                            CreateUserId = create_user.Id,
                            CreateUserName = create_user.FirstName + " " + create_user.LastName + " (" + create_user.Email + ")",

                            UpdateAt = userRole.UpdateAt,
                            UpdateUserId = userRole.CreateUserId,
                            UpdateUserName = userRole.UpdateUserId == null ? null : update_users.FirstName + " " + update_users.LastName + " (" + update_users.Email + ")",

                            DeleteAt = userRole.DeleteAt,
                            DeleteUserId = userRole.DeleteUserId,
                            DeleteUserName = userRole.DeleteUserId == null ? null : delete_users.FirstName + " " + delete_users.LastName + " (" + delete_users.Email + ")",

                            IsActive = userRole.IsActive,
                            IsDeleted = userRole.IsDeleted,
                        }).AsQueryable();

        return Task.FromResult(response);
    }
}
