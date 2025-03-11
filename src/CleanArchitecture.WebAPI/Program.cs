using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using CleanArchitecture.WebAPI;
using CleanArchitecture.WebAPI.Controllers;
using CleanArchitecture.WebAPI.Modules;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OData;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

// response boyutlar�n� d���recek kod
builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

// kendi yazd���m�z registrar'lar� ekledik.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors( opt =>
{
    opt.AddPolicy("MyPolicy", builder =>
    {
        builder.SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // Faster Response Time and Less Load on the server --> cqrs i�in daha performansl� �al��mas�n� sa�l�yor.
    });
});
builder.Services.AddOpenApi(); // OpenApi ekliyoruz.
builder.Services.AddControllers().AddOData(opt => 
    opt
    .Select()
    .Filter()
    .Count()
    .Expand()
    .OrderBy()
    .SetMaxTop(null)
    //.EnableQueryFeatures()
    .AddRouteComponents("odata",AppODataController.GetEdmModel())
    );
builder.Services.AddRateLimiter(x => x.AddFixedWindowLimiter("fixed", cfg =>
{
    cfg.QueueLimit = 100; // en fazla 100 istek kuyru�a al�n�r.
    cfg.Window = TimeSpan.FromSeconds(1); // 1 saniyelik s�re� i�inde s�n�rland�rma uygulan�r.
    cfg.PermitLimit = 100; // her pencere i�inde en fazla 100 iste�e izin verilir.
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // kuyrukta ilk gelen ilk i�lenir FIFO(First in First out)
})); // Bana ayn� anda �ok istek at�lamas�n diye ekliyorum.

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

//app.MapDefaultEndpoints();

app.UseHttpsRedirection();//HTTPS korumas�n� aktif eder.(g�venlik)

app.UseCors(x => x
.AllowAnyHeader()
.AllowCredentials()
.AllowAnyMethod()
.SetIsOriginAllowed(t => true));

app.RegistrarRoutes(); // Mod�lleri burada �al��t�rd�k.

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();// art�k yaz�lacak t�m controllerlar yukarda olu�turdu�umuz fixed isimli rateLimiter tan�mlamas�na uyumlu olmal�


ExtensionsMiddleware.CreateFirstUser(app);


app.Run();
