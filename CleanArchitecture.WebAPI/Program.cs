using CleanArchitecture.Application;
using CleanArchitecture.Infrastructure;
using Microsoft.AspNetCore.RateLimiting;
using Scalar.AspNetCore;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);


// kendi yazd���m�z registrar'lar� ekledik.
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors();
builder.Services.AddOpenApi(); // OpenApi ekliyoruz.
builder.Services.AddControllers();
builder.Services.AddRateLimiter(x => x.AddFixedWindowLimiter("fixed", cfg =>
{
    cfg.QueueLimit = 100; // en fazla 100 istek kuyru�a al�n�r.
    cfg.Window = TimeSpan.FromSeconds(1); // 1 saniyelik s�re� i�inde s�n�rland�rma uygulan�r.
    cfg.PermitLimit = 100; // her pencere i�inde en fazla 100 iste�e izin verilir.
    cfg.QueueProcessingOrder = QueueProcessingOrder.OldestFirst; // kuyrukta ilk gelen ilk i�lenir FIFO(First in First out)
})); // Bana ayn� anda �ok istek at�lamas�n diye ekliyorum.

var app = builder.Build();

app.MapOpenApi();
app.MapScalarApiReference();

app.UseCors(
    x=>x.AllowAnyHeader()
    .AllowCredentials()
    .AllowAnyMethod()
    .SetIsOriginAllowed(t => true)
    );

app.MapControllers().RequireRateLimiting("fixed"); // art�k yaz�lacak t�m controllerlar yukarda olu�turdu�umuz fixed isimli rateLimiter tan�mlamas�na uyumlu olmal�.

app.Run();
