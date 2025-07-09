using Application.Utils;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

public abstract class Controller<TController>(IMediator mediator, ILogger<TController> logger) : ControllerBase
    where TController : ControllerBase
{
    protected ILogger<TController> Logger { get; } = logger;
    protected IMediator Mediator => mediator;

    protected Task<TResponse> Send<TResponse>(IRequest<TResponse> request)
        => Mediator.Send(request, HttpContext.RequestAborted);

    protected new async Task<IActionResult> Response<T>(Task<Result<T, Error>> response)
    {
        var awaited = await response;

        return awaited switch
        {
            { IsSuccess: true } => MapSuccessResponse(awaited.Value),
            _ => MapErrorResponse(awaited.Error)
        };
    }

    protected virtual IActionResult MapSuccessResponse<T>(T response) => Ok(response);

    protected virtual IActionResult MapErrorResponse(Error error) => error switch
    {
        ValidationError validationError => new ObjectResult(new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            Title = "One or more validation errors occurred.",
            Status = 400,
            Instance = Request.Path,
            Extensions =
                {
                    ["traceId"] = HttpContext.TraceIdentifier,
                    ["errors"] = new Dictionary<string, string[]>
                    {
                        [validationError.PropertyName] = [validationError.Message]
                    }
                }
        })
        {
            StatusCode = 400
        },
        _ => new ObjectResult(new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "Internal Server Error",
            Detail = error?.Message ?? "An unexpected error occurred.",
            Status = 500,
            Instance = Request.Path,
            Extensions = { ["traceId"] = HttpContext.TraceIdentifier }
        })
        {
            StatusCode = 500
        }
    };
}