using CleanArchitecture.Application.Employees;
using CleanArchitecture.Application.Roles;
using CleanArchitecture.Domain.AppRoles;
using CleanArchitecture.Domain.Common.Results;
using MediatR;

namespace CleanArchitecture.WebAPI.Modules.Roles;

public static class RoleModule
{
    public static void CreateRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/createRole").WithTags("Roles").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, RoleCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }

    public static void DeleteRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/deleteRole").WithTags("Roles").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, RoleDeleteCommand request , CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }

    public static void GetRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/getRole").WithTags("Roles");

        group.MapGet(string.Empty,
            async (ISender sender,
            Guid id,
            CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new EmployeeGetQuery(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<AppRole>>();
    }

    public static void UpdateRoleRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("updateRole").WithTags("Roles").RequireAuthorization();

        group.MapPost(string.Empty,
            async (ISender sender, RoleUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}
