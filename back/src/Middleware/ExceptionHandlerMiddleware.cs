using System.Text.Json;
using Microsoft.AspNetCore.Mvc;

internal class ExceptionHandlerMiddleware : IMiddleware
{
    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.StatusCode = StatusCodes.Status400BadRequest;
        context.Response.ContentType = "application/json";

        var problem = new ProblemDetails()
        {
            Title = "Bad request",
            Status = StatusCodes.Status400BadRequest,
        };

        var json = JsonSerializer.Serialize(problem);

        await context.Response.WriteAsync(json);
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(context, ex);
        }
    }
}
