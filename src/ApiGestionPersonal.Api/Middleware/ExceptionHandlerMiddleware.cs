using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace ApiGestionPersonal.Api.Middleware;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred: {Message}", ex.Message);
            await HandleExceptionAsync(context, ex);
        }
    }

    private static async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        
        var response = exception switch
        {
            KeyNotFoundException => new { StatusCode = 404, Message = exception.Message },
            ArgumentException => new { StatusCode = 400, Message = exception.Message },
            UnauthorizedAccessException => new { StatusCode = 401, Message = "Unauthorized" },
            InvalidOperationException => new { StatusCode = 409, Message = exception.Message },
            _ => new { StatusCode = 500, Message = "An internal error occurred" }
        };

        context.Response.StatusCode = response.StatusCode;

        var jsonResponse = JsonSerializer.Serialize(new
        {
            error = response.Message,
            statusCode = response.StatusCode
        });

        await context.Response.WriteAsync(jsonResponse);
    }
}