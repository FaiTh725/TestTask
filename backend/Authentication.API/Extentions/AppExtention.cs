using Authentication.API.Validators.User;
using Authentication.Application.Model.User;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Authentication.API.Extentions
{
    public static class AppExtention
    {
        public static void AddValidators(this IServiceCollection service)
        {
            service.AddFluentValidationAutoValidation();
            service.AddScoped<IValidator<UserRequest>, UserRequestValidator>();
        }

        public static void AddCorses(this IServiceCollection service, IConfiguration configuration) 
        {
            var clientUrl = configuration
                .GetValue<string>("ApiList:Client") ??
                throw new ArgumentException("Cleint cors doesnt configure");

            service.AddCors(conf => conf.AddPolicy("Client", policy =>
            {
                policy.WithOrigins(clientUrl);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            }));
        }
    }
}
