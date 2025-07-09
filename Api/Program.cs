using System.Text.Json.Serialization;
using Api.Extensions;
using Api.Middlewares;
using Application;
using Infrastructure;
using Microsoft.AspNetCore.Mvc.ApiExplorer;

var builder = WebApplication.CreateBuilder(args);
var persistenceOptions = PersistenceOptions.Sqlite;

builder.AddCache();
builder.Services.AddControllers()
                .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });
builder.Services.AddVersioning();
builder.Services.AddProblemDetails();
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());
builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(persistenceOptions);
builder.Services.AddSwagger();
builder.Services.AddBreweryInitializationService();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        var provider = app.Services.GetRequiredService<IApiVersionDescriptionProvider>();
        foreach (var desc in provider.ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{desc.GroupName}/swagger.json", desc.GroupName.ToUpperInvariant());
        }
        options.RoutePrefix = string.Empty;
    });
}
app.UseMiddleware<ApiKeyMiddleware>();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.AddExceptionHandler();

if (persistenceOptions == PersistenceOptions.Sqlite)
{
    app.Services.GetRequiredService<IServiceScopeFactory>().MigrateDatabase();
}

app.Run();