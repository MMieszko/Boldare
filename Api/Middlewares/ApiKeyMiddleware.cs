namespace Api.Middlewares;

internal class ApiKeyMiddleware(RequestDelegate next, IConfiguration config)
{
    private const string ApiKeyHeader = "X-Api-Key";
    private readonly string _expectedApiKey = config["ApiKey"]!;

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Headers.TryGetValue(ApiKeyHeader, out var extractedKey) || extractedKey != _expectedApiKey)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Invalid or missing API Key");

            return;
        }

        await next(context);
    }
}