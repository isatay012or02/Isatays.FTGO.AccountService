using FluentValidation;
using Isatays.FTGO.AccountService.Api.Common.Errors;
using Isatays.FTGO.AccountService.Api.Common.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Net;
using System.Text.Json;

namespace Isatays.FTGO.AccountService.Api.Common.Extensions;

[ExcludeFromCodeCoverage]
public static class GlobalExceptionHandlerExtensions
{
    public static IApplicationBuilder UseGlobalExceptionHandler(this IApplicationBuilder app)
    {
        _ = app.UseExceptionHandler(appError => appError.Run(async context =>
        {
            var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

            if (contextFeature != null)
            {
                // Set the Http Status Code
                var statusCode = contextFeature.Error switch
                {
                    ValidationException ex => HttpStatusCode.BadRequest,
                    NotFoundException ex => HttpStatusCode.NotFound,
                    _ => HttpStatusCode.InternalServerError
                };

                // Prepare Generic Error
                var apiError = new ApiError(contextFeature.Error.Message, contextFeature.Error.InnerException?.Message, contextFeature.Error.StackTrace);

                // Set Response Details
                context.Response.StatusCode = (int)statusCode;
                context.Response.ContentType = "application/json";

                // Return the Serialized Generic Error
                await context.Response.WriteAsync(JsonSerializer.Serialize(apiError));
            }
        }));

        return app;
    }
}
