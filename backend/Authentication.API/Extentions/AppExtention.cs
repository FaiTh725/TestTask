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
    }
}
