internal class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;

    public ExceptionHandlerMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task Invoke(HttpContext httpContext)
    {
        try
        {
            await _next(httpContext);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext httpContext, Exception exception)
    {
        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        await httpContext.Response.WriteAsync("An exception was thrown.");
    }
}

internal static class ExceptionHandlerMiddlewareExtensions
{
    internal static IApplicationBuilder UserCustomExeptionHandler(this IApplicationBuilder applicationBuilder)
        => applicationBuilder.UseMiddleware<ExceptionHandlerMiddleware>();
}
