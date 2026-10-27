namespace CUSTOMMIDDLEWARE;

public static class MiddlewareExtensions
{
    public static IApplicationBuilder UseRequestProfiler(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<RequestProfilerMiddleware>();
    }
}