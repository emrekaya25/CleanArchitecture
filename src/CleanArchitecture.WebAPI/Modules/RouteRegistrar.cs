using CleanArchitecture.WebAPI.Modules.Users;

namespace CleanArchitecture.WebAPI.Modules;

public static class RouteRegistrar
{
    // Oluşturduğumuz modülleri burada bağlıyoruz.
    public static void RegistrarRoutes(this IEndpointRouteBuilder app)
    {
        app.RegisterUserRoutes();
    }
}

