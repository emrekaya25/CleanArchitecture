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

// response boyutlarýný düþürecek kod
builder.Services.AddResponseCompression(opt =>
{
    opt.EnableForHttps = true;
});

// kendi yazdýðýmýz registrar'larý ekledik.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors( opt =>
{
    opt.AddPolicy("MyPolicy", builder =>
    {
        builder.SetPreflightMaxAge(TimeSpan.FromMinutes(10)); // Faster Response Time and Less Load on the server --> cqrs için daha performanslý çalýþmasýný saðlýyor.
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
    cfg.QueueLimit = 100; // en fazla 100 istek kuyruða alýnýr.
    cfg.Window = TimeSpan.FromSeconds(1); // 1 saniyelik süreç içinde sýnýrlandýrma uygulanýr.
    cfg.PermitLimit = 100; // her pencere içinde en fazla 100 isteðe izin verilir.
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // kuyrukta ilk gelen ilk iþlenir FIFO(First in First out)
})); // Bana ayný anda çok istek atýlamasýn diye ekliyorum.

builder.Services.AddExceptionHandler<ExceptionHandler>().AddProblemDetails();

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

//app.MapDefaultEndpoints();

app.UseHttpsRedirection();//HTTPS korumasýný aktif eder.(güvenlik)

app.UseCors(x => x
.AllowAnyHeader()
.AllowCredentials()
.AllowAnyMethod()
.SetIsOriginAllowed(t => true));

app.RegistrarRoutes(); // Modülleri burada çalýþtýrdýk.

app.UseAuthentication();
app.UseAuthorization();

app.UseResponseCompression();

app.UseExceptionHandler();

app.MapControllers().RequireRateLimiting("fixed").RequireAuthorization();// artýk yazýlacak tüm controllerlar yukarda oluþturduðumuz fixed isimli rateLimiter tanýmlamasýna uyumlu olmalý


ExtensionsMiddleware.CreateFirstUser(app);


app.Run();
