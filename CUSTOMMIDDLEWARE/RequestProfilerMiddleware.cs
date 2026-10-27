using System.Diagnostics;
using Microsoft.Extensions.Configuration;

namespace CUSTOMMIDDLEWARE;

public class RequestProfilerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestProfilerMiddleware> _logger;
    private readonly IConfiguration _configuration;

    public RequestProfilerMiddleware(RequestDelegate next, ILogger<RequestProfilerMiddleware> logger, IConfiguration configuration)
    {
        _next = next;
        _logger = logger;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var traceId = Guid.NewGuid().ToString("N")[..8];

        _logger.LogInformation(
            "[{TraceId}] Incoming Request: {Method} {Path}",
            traceId, context.Request.Method, context.Request.Path);

        // Configuration-driven API Key Validation
        if (context.Request.Path.StartsWithSegments("/api/secure"))
        {
            var expectedApiKey = _configuration["SecuritySettings:ApiKey"];
            if (!context.Request.Headers.TryGetValue("X-Api-Key", out var providedKey) || providedKey != expectedApiKey)
            {
                _logger.LogWarning("[{TraceId}] Unauthorized request rejected by middleware.", traceId);
                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                await context.Response.WriteAsync("Unauthorized: Invalid or missing X-Api-Key header.");
                return; 
            }
        }

        // Attach Trace ID early to the response
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("X-Trace-Id", traceId);
            return Task.CompletedTask;
        });

        // Pass control down the pipeline
        await _next(context);

        stopwatch.Stop();

        // Inject performance header on the way out
        context.Response.Headers["X-Execution-Time-Ms"] = stopwatch.ElapsedMilliseconds.ToString();

        _logger.LogInformation(
            "[{TraceId}] Outgoing Response: Status {StatusCode} | Executed in {ElapsedMilliseconds}ms",
            traceId, context.Response.StatusCode, stopwatch.ElapsedMilliseconds);
    }
}