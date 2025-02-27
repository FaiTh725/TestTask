using Application.Shared.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Middlewares
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IHostApplicationLifetime application;
        private readonly ILogger<ExceptionHandlerMiddleware> logger;
        private readonly IProblemDetailsService problemDetailsService;

        public ExceptionHandlerMiddleware(
            RequestDelegate next,
            IHostApplicationLifetime application,
            ILogger<ExceptionHandlerMiddleware> logger,
            IProblemDetailsService problemDetailsService)
        {
            this.next = next;
            this.application = application;
            this.logger = logger;
            this.problemDetailsService = problemDetailsService;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await next(httpContext);
            }
            catch(Exception ex)
            {
                await TryHandleAsync(httpContext, ex, httpContext.RequestAborted);
            }
        }

        private async ValueTask<bool> TryHandleAsync(
            HttpContext context,
            Exception exception,
            CancellationToken cancellationToken)
        {
            if(exception is AplicationConfigurationException appConf)
            {
                logger.LogError("Error With Configuration." +
                    " Section With Error " + appConf.ErrorConfigurationSection);
                application.StopApplication();
            }

            context.Response.StatusCode = exception switch
            {
                NotFoundApiException => StatusCodes.Status404NotFound,
                BadRequestApiException => StatusCodes.Status400BadRequest,
                ConflictApiException => StatusCodes.Status409Conflict,
                InternalServerApiException => StatusCodes.Status500InternalServerError,
                _ => StatusCodes.Status500InternalServerError
            };

            return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = context,
                ProblemDetails = new ProblemDetails
                {
                    Type = exception.GetType().Name,
                    Title = "Error iccured",
                    Detail = exception.Message,
                    Instance = $"{context.Request.Method} {context.Request.Path}"
                }
            });
        }
    }
}
