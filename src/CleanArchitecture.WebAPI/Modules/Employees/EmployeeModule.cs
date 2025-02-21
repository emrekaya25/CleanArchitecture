using CleanArchitecture.Application.Employees;
using CleanArchitecture.Domain.Common.Results;
using CleanArchitecture.Domain.Employees;
using MediatR;

namespace CleanArchitecture.WebAPI.Modules.Employees;

public static class EmployeeModule
{
    //Burası user için minimalAPI yazacağımız sınıfımız. (ekleme-silme-güncelleme işlemlerini minimalAPI ile hızlı bir şekilde gerçekleştireceğiz.)
    public static void RegisterEmployeeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/registerUser").WithTags("Users").RequireAuthorization();//giriş zorunlu bu işlemi yapması için

        group.MapPost(string.Empty,
            async (ISender sender, EmployeeCreateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request, cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>(); // scalarda body kısmı gözükmesi için produces yazdık.
    }

    public static void DeleteEmployeeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/deleteUser").WithTags("Users");

        group.MapPost(string.Empty,
            async (ISender sender, EmployeeDeleteCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request,cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>> ();
    }

    public static void UpdateEmployeeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/updateUser").WithTags("Users");

        group.MapPost(string.Empty,
            async (ISender sender, EmployeeUpdateCommand request, CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(request,cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<string>>();
    }

    public static void GetEmployeeRoutes(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/getUser").WithTags("Users");

        group.MapGet(string.Empty,
            async (ISender sender,
            Guid id,
            CancellationToken cancellationToken) =>
            {
                var response = await sender.Send(new EmployeeGetQuery(id), cancellationToken);
                return response.IsSuccessful ? Results.Ok(response) : Results.InternalServerError(response);
            })
            .Produces<Result<Employee>>();
    }
}

