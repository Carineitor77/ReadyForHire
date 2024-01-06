using System.Net;
using Domain.Exceptions;

namespace API.Middlewares;

public class GlobalExceptionHandlerMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception ex)
        {
            await HandleException(context, ex);
        }
    }
    
    private static async Task HandleException(HttpContext context, Exception ex)
    {
        context.Response.ContentType = "text";

        if (ex is ApiException apiEx)
        {
            context.Response.StatusCode = (int)apiEx.StatusCode;
        }
        else
        {
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        }
        
        await context.Response.WriteAsync(ex.Message);
    }
}