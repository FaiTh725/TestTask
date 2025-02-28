using Authentication.API.Validators.User;
using Authentication.Application;
using Authentication.Application.Commands.User.Register;
using FluentValidation;
using FluentValidation.AspNetCore;

namespace Authentication.API.Extentions
{
    public static class AppExtention
    {
        public static void AddValidators(this IServiceCollection service)
        {
            service.AddFluentValidationAutoValidation();
            service.AddScoped<IValidator<RegisterUserCommand>, UserRegisterValidator>();
        }

        public static void ConfigureMediatR(this IServiceCollection services)
        {
            services.AddMediatR(cfg => 
            cfg.RegisterServicesFromAssembly(typeof(AppAssemblyReference).Assembly));
        }

        // Implement Proxy(Yarp or using Nginx)
        public static void AddCorses(this IServiceCollection service, IConfiguration configuration) 
        {
            var clientUrlHttp = configuration
                .GetValue<string>("ApiList:ClientHttp") ??
                throw new ArgumentException("Cleint cors doesnt configure");
            var clientUrlHttps = configuration
                .GetValue<string>("ApiList:ClientHttps") ??
                throw new ArgumentException("Cleint cors doesnt configure");

            service.AddCors(conf => conf.AddPolicy("Client", policy =>
            {
                policy.WithOrigins(clientUrlHttp, clientUrlHttps);
                policy.AllowAnyHeader();
                policy.AllowAnyMethod();
                policy.AllowCredentials();
            }));
        }
    }
}
