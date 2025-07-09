using Api.Models.Requests;
using Api.Models.Responses;
using Application.Queries.SearchBrewery;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using CSharpFunctionalExtensions;

namespace Api.Controllers.V1;

[ApiController]
[ApiVersion("1.0")]
[Route("api/v{version:apiVersion}/brewery")]
public class BreweryV1Controller(IMediator mediator, ILogger<BreweryV1Controller> logger) : Controller<BreweryV1Controller>(mediator, logger)
{
    [HttpGet("search")]
    [ProducesResponseType(typeof(BreweryModel[]), StatusCodes.Status200OK)]
    public Task<IActionResult> Search([FromQuery] BrewerySearchRequestModel request)
         => Response(Send(new SearchBreweryQuery(request.Query)).Map(breweries => breweries.Select(BreweryModel.FromApplication)));
}