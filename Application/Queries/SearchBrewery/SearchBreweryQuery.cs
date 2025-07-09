using Application.Models;
using Application.Utils;
using CSharpFunctionalExtensions;
using MediatR;

namespace Application.Queries.SearchBrewery;

public record SearchBreweryQuery(string Query) : IRequest<Result<IReadOnlyCollection<Brewery>, Error>>;
