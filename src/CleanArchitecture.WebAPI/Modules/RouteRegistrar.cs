using CleanArchitecture.WebAPI.Modules.Auth;
using CleanArchitecture.WebAPI.Modules.Users;

namespace CleanArchitecture.WebAPI.Modules;

public static class RouteRegistrar
{
    // Oluşturduğumuz modülleri burada bağlıyoruz.
    public static void RegistrarRoutes(this IEndpointRouteBuilder app)
    {
        //User
        app.RegisterUserRoutes();
        app.DeleteUserRoutes();
        app.UpdateUserRoutes();
        app.GetUserRoutes();

        //Auth
        app.RegisterAuthRoutes();
    }
}

