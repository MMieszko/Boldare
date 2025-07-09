using Api.Models.Requests;
using Api.Models.Responses;
using Application.Queries.SearchAndSortBrewery;
using CSharpFunctionalExtensions;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers.V2;

[ApiController]
[ApiVersion("2.0")]
[Route("api/v{version:apiVersion}/brewery")]
public class BreweryV2Controller(IMediator mediator, ILogger<BreweryV2Controller> logger) : Controller<BreweryV2Controller>(mediator, logger)
{
    [HttpGet("search")]
    [ProducesResponseType(typeof(BreweryModel[]), StatusCodes.Status200OK)]
    public Task<IActionResult> Get([FromQuery] BrewerySearchRequest request)
        => Response(Send(new SearchAndSortBreweryQuery(request.Query, request.SortBy, request.GeoLocation()))
            .Map(breweries => breweries.Select(BreweryModel.FromApplication)));

}