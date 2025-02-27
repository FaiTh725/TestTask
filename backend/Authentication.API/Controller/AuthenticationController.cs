using Authentication.Application.Commands.User.Login;
using Authentication.Application.Commands.User.Register;
using Authentication.Application.Interfaces;
using Authentication.Application.Queries.User.GetUserByEmail;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IMediator mediator;
        private readonly IAuthTokenService tokenService;

        public AuthenticationController(
            IMediator mediator,
            IAuthTokenService tokenService)
        {
            this.mediator = mediator;
            this.tokenService = tokenService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login([FromQuery] LoginUserCommand command)
        {
            var userEmail = await mediator.Send(command);

            var userData = await mediator.Send(new GetUserByEmailQuery
            {
                Email = userEmail.ToString()
            });

            var token = tokenService.GenerateToken(userData);

            var cookiesOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true
            };

            Response.Cookies.Append("token",
                token,
                cookiesOptions);

            return Ok(userData);
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            var newUserEmail = await mediator.Send(command);

            var userData = await mediator.Send(new GetUserByEmailQuery
            {
                Email = newUserEmail
            });

            var token = tokenService.GenerateToken(userData);

            var cookiesOptions = new CookieOptions
            {
                Secure = true,
                SameSite = SameSiteMode.None,
                HttpOnly = true
            };

            Response.Cookies.Append("token",
                token,
                cookiesOptions);

            return Ok(userData);
        }
    }
}
