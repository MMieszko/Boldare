using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Extensions;

internal static class ExceptionHandlerExtensions
{
    public static void AddExceptionHandler(this IApplicationBuilder applicationBuilder)
    {
        applicationBuilder.UseExceptionHandler(
            new ExceptionHandlerOptions
            {
                AllowStatusCode404Response = true,
                ExceptionHandler = async context =>
                {
                    var errorFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                    if (errorFeature?.Error != null)
                    {
                        logger.LogError(errorFeature.Error, "An unhandled exception occurred while processing the request.");

                        await context.Response.WriteAsJsonAsync(new ObjectResult(new ProblemDetails
                        {
                            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                            Title = "Internal Server Error",
                            Detail = errorFeature.Error.Message,
                            Status = 500,
                            Instance = context.Request.Path,
                            Extensions = { ["traceId"] = context.TraceIdentifier }
                        })
                        {
                            StatusCode = 500
                        });
                    }
                }
            });
    }
}