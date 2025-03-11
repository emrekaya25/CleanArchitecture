using CleanArchitecture.Application.Auth;
using CleanArchitecture.Application.Employees;
using CleanArchitecture.Application.Roles;
using CleanArchitecture.Application.UserRoles;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Microsoft.OData.Edm;
using Microsoft.OData.ModelBuilder;

namespace CleanArchitecture.WebAPI.Controllers;

[Route("odata")]
[ApiController]
[EnableQuery] //OData'da sorgu yapabilmek için bunu ekliyoruz.
public class AppODataController(ISender sender) : ODataController
{
    public static IEdmModel GetEdmModel()
    {
        ODataConventionModelBuilder builder = new();
        builder.EnableLowerCamelCase();

        builder.EntitySet<EmployeeGetAllQueryResponse>("employees"); // altta yazdığın endpointin ismini en sona dönüş tipinide EntitySet'in içine yazıyosun.
        builder.EntitySet<UserGetAllQueryResponse>("users");
        builder.EntitySet<RoleGetAllQueryResponse>("roles");
        builder.EntitySet<UserRoleGetAllQueryResponse>("userRoles");

        return builder.GetEdmModel();
    }

    [HttpGet("employees")]
    //Bu şekilde direk yaptığımızda OData çalışıyor fakat böyle yaptığımız zaman bize toplam sayıyı vermiyor bu yüzden program.cs'e gidip OData'nın yapısına bazı seçenekler ekliyoruz.
    public async Task<IQueryable<EmployeeGetAllQueryResponse>> GetAllEmployees(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new EmployeeGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("users")]
    public async Task<IQueryable<UserGetAllQueryResponse>> GetAllUsers(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("roles")]
    public async Task<IQueryable<RoleGetAllQueryResponse>> GetAllRoles(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new RoleGetAllQuery(), cancellationToken);
        return response;
    }

    [HttpGet("userRoles")]
    public async Task<IQueryable<UserRoleGetAllQueryResponse>> GetAllUserRoles(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserRoleGetAllQuery(), cancellationToken);
        return response;
    }
}

