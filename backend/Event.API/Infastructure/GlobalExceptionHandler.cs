﻿using Application.Shared.Exceptions;
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
                Title = "API Error",
                Detail = "Internal Server Error",
                Instance = "API",
                Extensions = new Dictionary<string, object?>()
            };

            if(exception is AplicationConfigurationException confEx)
            {
                logger.LogError(confEx, "Error with configuration application." +
                    "Section with error " + confEx.ErrorConfigurationSection);

                application.StopApplication();
            }
            else
            {
                logger.LogError(exception, "Inner api exception - " + 
                    exception.Message);
            }

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await httpContext.Response.WriteAsJsonAsync(problemDetails, cancellationToken);

            return true;
        }
    }
}
