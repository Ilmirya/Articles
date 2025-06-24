using System.Net.Mime;
using System.Text.Json;
using Articles.Api.Models.Responses;
using Articles.Application;
using Articles.Application.Exceptions;
using Microsoft.AspNetCore.Diagnostics;

namespace Articles.Api.Infrastructure;

internal static class ExceptionHandlerExtensions
{
    public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        => app
            .UseExceptionHandler(
                exceptionHandlerApp
                    => exceptionHandlerApp.Run(
                        async context =>
                        {
                            var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                            var exception = exceptionHandlerPathFeature?.Error;
                            
                            var (statusCode, message) = exception switch
                            {
                                ObjectNotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                                ArgumentException => (StatusCodes.Status400BadRequest, exception.Message),
                                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred")
                            };
                            
                            context.Response.StatusCode = statusCode;
                            context.Response.ContentType = MediaTypeNames.Application.Json;
                            await context.Response.WriteAsync(JsonSerializer.Serialize(new ErrorResponse(message)));
                        }));
}