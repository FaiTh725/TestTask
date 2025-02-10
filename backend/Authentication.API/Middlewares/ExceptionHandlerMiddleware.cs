using Application.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace Authentication.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostApplicationLifetime application;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            IHostApplicationLifetime application,
            ILogger<ExceptionHandlerMiddleware> logger)
        {
            this.next = next;
            this.application = application;
            this.logger = logger;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch(ApplicationConfigurationException appEx)
            {
                logger.LogError(appEx, "Error With Configuration." +
                    "Section With Error " + appEx.ConfigurationErrorSection);

                await HandleException(httpContext, appEx);

                application.StopApplication();
            }
            catch(Exception ex)
            {
                logger.LogError(ex, "Error With Configuration.");

                await HandleException(httpContext, ex);
            }
        }

        private static async Task HandleException(HttpContext httpContext, Exception ex)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "API Error",
                Detail = "Internal Server Error",
                Instance = "API"
            };

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails);
        }
    }
}
