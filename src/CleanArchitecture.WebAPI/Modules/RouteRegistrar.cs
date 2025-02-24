using CleanArchitecture.WebAPI.Modules.Auth;
using CleanArchitecture.WebAPI.Modules.Employees;

namespace CleanArchitecture.WebAPI.Modules;

public static class RouteRegistrar
{
    // Oluşturduğumuz modülleri burada bağlıyoruz.
    public static void RegistrarRoutes(this IEndpointRouteBuilder app)
    {
        //User
        app.RegisterEmployeeRoutes();
        app.DeleteEmployeeRoutes();
        app.UpdateEmployeeRoutes();
        app.GetEmployeeRoutes();

        //Auth
        app.RegisterAuthRoutes();
        app.CreateUserRoutes();
        app.GetUserRoutes();
        app.UserChangePasswordRoutes();
        app.UserForgotPasswordRoutes();
        app.UserChangeForgotPasswordRoutes();
    }
}

