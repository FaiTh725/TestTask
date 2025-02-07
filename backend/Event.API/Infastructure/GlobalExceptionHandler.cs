using Application.Shared.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Event.API.Infastructure
{
    public class GlobalExceptionHandler : IExceptionHandler
    {
        private readonly ILogger<GlobalExceptionHandler> logger;
        private readonly IHostApplicationLifetime application;

        public GlobalExceptionHandler(
            ILogger<GlobalExceptionHandler> logger,
            IHostApplicationLifetime application)
        {
            this.logger = logger;
            this.application = application;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext, 
            Exception exception, 
            CancellationToken cancellationToken)
        {
            var problemDetails = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "Internal Server Error",
            };

            if(exception is ApplicationConfigurationException confEx)
            {
                logger.LogError(confEx, "Error with configuration application." +
                    "Section with error " + confEx.ConfigurationErrorSection);

                application.StopApplication();
            }
            else
            {
                logger.LogError(exception, "Inner api exception - " + 
                    exception.Message);
            }

            httpContext.Response.ContentType = "application/json";
            await httpContext.Response.WriteAsJsonAsync(problemDetails);

            return true;
        }
    }
}
