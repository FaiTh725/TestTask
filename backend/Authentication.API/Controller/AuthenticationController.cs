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
        public async Task<IActionResult> Login(
            [FromQuery]LoginUserCommand command,
            CancellationToken cancellationToken)
        {
            var userEmail = await mediator.Send(command, cancellationToken);

            var userData = await mediator.Send(new GetUserByEmailQuery
            {
                Email = userEmail.ToString()
            },
            cancellationToken);

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
        public async Task<IActionResult> Register(
            RegisterUserCommand command,
            CancellationToken cancellationToken)
        {
            var newUserEmail = await mediator.Send(command, cancellationToken);

            var userData = await mediator.Send(new GetUserByEmailQuery
            {
                Email = newUserEmail
            }, cancellationToken);

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
