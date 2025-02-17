using CleanArchitecture.Domain.Common.Results;
using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;
using System.Text.Json;

namespace CleanArchitecture.WebAPI
{
    public sealed class ExceptionHandler : IExceptionHandler
    { // Tüm hataları tek bir kalıp haline getiriyoruz. Kendi yazdığımız Result pattern haline böylelikle UI tarafında tek bir pattern kullanabileceğiz.
        public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
        {
            Result<string> errorResult;

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = 500;

            if (exception.GetType() == typeof(ValidationException))
            {
                httpContext.Response.StatusCode = 403;
                errorResult = Result<string>.Failure(403, ((ValidationException)exception).Errors.Select(s => s.PropertyName).ToList()); //ValidationException'dan(FluentValidation) gelen hataları errorResult içerisine atıyoruz.

                await httpContext.Response.WriteAsJsonAsync(errorResult);
                return true;
            }

            errorResult = Result<string>.Failure(exception.Message); // Normal errorlarıda burada içerisine atıyoruz.

            await httpContext.Response.WriteAsJsonAsync(errorResult);

            return true;
        }
    }
}
