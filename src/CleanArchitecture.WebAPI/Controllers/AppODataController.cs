using CleanArchitecture.Application.Users;
using CleanArchitecture.Domain.Users;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
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
        builder.EntitySet<UserGetAllQueryResponse>("users"); // altta yazdığın endpointin ismini en sona dönüş tipinide EntitySet'in içine yazıyosun.
        return builder.GetEdmModel();
    }

    [HttpGet("users")]
    //Bu şekilde direk yaptığımızda OData çalışıyor fakat böyle yaptığımız zaman bize toplam sayıyı vermiyor bu yüzden program.cs'e gidip OData'nın yapısına bazı seçenekler ekliyoruz.
    public async Task<IQueryable<UserGetAllQueryResponse>> GetAllUsers(CancellationToken cancellationToken)
    {
        var response = await sender.Send(new UserGetAllQuery(), cancellationToken);
        return response;
    }
}

