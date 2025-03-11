using CleanArchitecture.WebAPI.Modules.Auth;
using CleanArchitecture.WebAPI.Modules.Employees;
using CleanArchitecture.WebAPI.Modules.Roles;
using CleanArchitecture.WebAPI.Modules.UserRoles;

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

        //Role
        app.CreateRoleRoutes();
        app.DeleteRoleRoutes();
        app.GetRoleRoutes();
        app.UpdateRoleRoutes();

        //UserRole
        app.CreateUserRoleRoutes();
        app.DeleteUserRoleRoutes();
        app.GetUserRoleRoutes();
        app.UpdateUserRoleRoutes();
    }
}

