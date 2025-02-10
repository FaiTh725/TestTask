using Application.Shared.Responses;
using Authentication.Application.Interfaces;
using Authentication.Application.Model.Token;
using Authentication.Application.Model.User;
using Microsoft.AspNetCore.Mvc;
using CustomStatusCode = Application.Shared.Enums.StatusCode;

namespace Authentication.API.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService authService;

        public AuthenticationController(IAuthService authService)
        {
            this.authService = authService;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> Login(string email, string password)
        {
            var request = new UserRequest 
            { 
                Email = email, 
                Password = password 
            };

            var response = await authService.LoginUser(request);

            if (response.StatusCode == CustomStatusCode.Ok)
            {
                Response.Cookies.Append("token", response.Data.Token);
            }

            return new JsonResult(new DataResponse<TokenResponse>
            {
                StatusCode = response.StatusCode,
                Description = response.Description,
                Data = response.Data.TokenResponse ??
                    new TokenResponse()
            });
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Register(UserRequest request)
        {
            var response = await authService.RegisterUser(request);

            if (response.StatusCode == CustomStatusCode.Ok)
            {
                Response.Cookies.Append("token", response.Data.Token);
            }

            return new JsonResult(new DataResponse<TokenResponse> 
            {
                StatusCode = response.StatusCode,
                Description = response.Description,
                Data = response.Data.TokenResponse ?? 
                    new TokenResponse()
            });
        }
    }
}
