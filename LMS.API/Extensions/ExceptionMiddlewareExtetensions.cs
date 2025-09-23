using Domain.Models.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace LMS.API.Extensions;

public static class ExceptionMiddlewareExtetensions
{
    public static void ConfigureExceptionHandler(this WebApplication app)
    {
        app.UseExceptionHandler(builder =>
        {
            builder.Run(async context =>
            {
                var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                if (contextFeature != null)
                {
                    var problemDetailsFactory = app.Services.GetRequiredService<ProblemDetailsFactory>();

                    ProblemDetails problemDetails;
                    int statusCode;

                    switch (contextFeature.Error)
                    {

                        case NotFoundException notFoundException:
                            statusCode = StatusCodes.Status404NotFound;
                            problemDetails = problemDetailsFactory.CreateProblemDetails(
                                context,
                                statusCode,
                                title: notFoundException.Title,
                                detail: notFoundException.Message,
                                instance: context.Request.Path);
                            break;

                        case SaveFailureException saveFailureException:
                            statusCode = saveFailureException.StatusCode;
                            problemDetails = problemDetailsFactory.CreateProblemDetails(
                                context,
                                statusCode,
                                title: saveFailureException.Title,
                                detail: saveFailureException.Message,
                                instance: context.Request.Path);
                            break;

                        case TokenValidationException tokenValidationException:
                            statusCode = tokenValidationException.StatusCode;
                            problemDetails = problemDetailsFactory.CreateProblemDetails(
                                    context,
                                    statusCode,
                                    detail: tokenValidationException.Message,
                                    instance: context.Request.Path);
                            break;
                        default:
                            statusCode = StatusCodes.Status500InternalServerError;
                            problemDetails = problemDetailsFactory.CreateProblemDetails(
                                    context,
                                    statusCode,
                                    title: "Internal Server Error",
                                    detail: contextFeature.Error.Message,
                                    instance: context.Request.Path);
                            break;
                    }

                    context.Response.StatusCode = statusCode;
                    await context.Response.WriteAsJsonAsync(problemDetails);
                }
            });
        });
    }
}
