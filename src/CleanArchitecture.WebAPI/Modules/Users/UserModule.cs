using CleanArchitecture.Application.Users;
using CleanArchitecture.Domain.Common.Results;
using MediatR;

namespace CleanArchitecture.WebAPI.Modules.Users;

public static class UserModule
{
    //Burası user için minimalAPI yazacağımız sınıfımız. (ekleme-silme-güncelleme işlemlerini minimalAPI ile hızlı bir şekilde gerçekleştireceğiz.)
    public static void RegisterUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/registerUser").WithTags("Users");

        group.MapPost(string.Empty,
            async (ISender sender, UserCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>(); // scalarda body kısmı gözükmesi için produces yazdık.
    }

    public static void DeleteUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/deleteUser").WithTags("Users");

        group.MapPost(string.Empty,
            async (ISender sender, UserDeleteCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request,cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>> ();
    }

    public static void UpdateUserRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/updateUser").WithTags("Users");

        group.MapPost(string.Empty,
            async (ISender sender, UserUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request,cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }
}

