using CleanArchitecture.Application.UserRoles;
using CleanArchitecture.Domain.Common.Results;
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
}
