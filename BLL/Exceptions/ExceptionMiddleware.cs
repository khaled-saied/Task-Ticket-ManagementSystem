using System.Text.Json;
using Microsoft.AspNetCore.Http;

namespace BLL.Exceptions
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionMiddleware(RequestDelegate next) => _next = next;

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (NotFoundException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status404NotFound, ex.Message);
            }
            catch (ConflictException ex)
            {
                await HandleExceptionAsync(context, StatusCodes.Status409Conflict, ex.Message);
            }
            catch (Exception ex)
            {
                await HandleInternalServerError(context, ex);
            }
        }

        private static async Task HandleInternalServerError(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;

            var response = new { error = ex.Message };
            var json = System.Text.Json.JsonSerializer.Serialize(response);
            await context.Response.WriteAsync(json);
        }

        private static async Task HandleExceptionAsync(HttpContext context, int statusCode, string message)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = statusCode;

            var result = JsonSerializer.Serialize(new { error = message });
            await context.Response.WriteAsync(result);
        }
    }
}