using CleanArchitecture.Application.UserRoles;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.UserRoles;
using MediatR;

namespace CleanArchitecture.WebAPI.Modules.UserRoles;

public static class UserRoleModule
{
    public static void CreateUserRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/createUserRole").WithTags("UserRoles").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, UserRoleCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }

    public static void DeleteUserRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/deleteUserRole").WithTags("UserRoles").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, UserRoleDeleteCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }

    public static void GetUserRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/getUserRole").WithTags("UserRoles").RequireAuthorization();

        group.MapGet(string.Empty,
            async (ISender sender, Guid id, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new UserRoleGetQuery(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<UserRole>>();
    }

    public static void UpdateUserRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/updateUserRole").WithTags("UserRoles").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, UserRoleUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response.ErrorMessages);
            })
            .Produces<Result<UserRole>>();
    }
}
