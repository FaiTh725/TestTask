using Application.Shared.Enums;
using Application.Shared.Responses;
using Authentication.Application.Interfaces;
using Authentication.Application.Model.Token;
using Authentication.Application.Model.User;
using Authentication.Domain.Common;
using Authentication.Domain.Entities;
using Authentication.Domain.Repositories;

namespace Authentication.Application.Implmentations
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository userRepository;
        private readonly IRoleRepository roleRepository;
        private readonly IAuthTokenService tokenService;
        private readonly IHashService hashService;
        private readonly IDBTransaction dbTransaction;

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository, 
            IAuthTokenService tokenService,
            IHashService hashService,
            IDBTransaction dbTransaction)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.tokenService = tokenService;
            this.hashService = hashService; 
            this.dbTransaction = dbTransaction;
        }

        public async Task<DataResponse<UserResponse>> LoginUser(UserRequest request)
        {
            var user = await userRepository.GetUser(request.Email);

            if (user.IsFailure)
            {
                return new DataResponse<UserResponse>
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Current Email Does Not Exist",
                    Data = new()
                };
            }

            if(!hashService.VerifyHash(request.Password, user.Value.Password))
            {
                return new DataResponse<UserResponse>
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Incorrect Password Or Email",
                    Data = new()
                };
            }

            var token = tokenService.GenerateToken(user.Value);
            var tokenData = tokenService.DecodeToken(token);

            if (tokenData.IsFailure)
            {
                return new DataResponse<UserResponse> 
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Inknown Server Error",
                    Data = new()
                };
            }

            return new DataResponse<UserResponse> 
            {
                StatusCode = StatusCode.Ok,
                Description = "User Enter",
                Data = new UserResponse
                {
                    Token = token,
                    TokenResponse = new TokenResponse
                    {
                        Email = tokenData.Value.Email,
                        Role = tokenData.Value.Role,
                    }
                }
            };
        }

        public async Task<DataResponse<UserResponse>> RegisterUser(UserRequest request)
        {
            var user = await userRepository.GetUser(request.Email);

            if (user.IsSuccess)
            {
                return new DataResponse<UserResponse>
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Current Email Already Registered",
                    Data = new()
                };
            }

            var isCorrectPassword = User.CheckPasswordForValid(request.Password);

            if(!isCorrectPassword)
            {
                return new DataResponse<UserResponse>
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Password Should Has One Letter And One Number",
                    Data = new()
                };
            }

            var userRole = await roleRepository.GetRole("User");

            if (userRole.IsFailure)
            {
                return new DataResponse<UserResponse> 
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unknown Server Error",
                    Data = new() 
                };
            }

            var passwordHash = hashService.GenerateHash(request.Password);

            var initUser = User.Initialize(
                request.Email, 
                passwordHash,
                userRole.Value);

            if(initUser.IsFailure)
            {
                return new DataResponse<UserResponse>
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Invalid Request - " + initUser.Error,
                    Data = new()
                };      
            }

            await dbTransaction.StartTransaction();

            var newUser = await userRepository.AddUser(initUser.Value);

            if(newUser.IsFailure)
            {
                return new DataResponse<UserResponse>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unknown Server Error",
                    Data = new()
                };
            }

            var token = tokenService.GenerateToken(newUser.Value);

            var tokenData = tokenService.DecodeToken(token);

            if(tokenData.IsFailure)
            {
                await dbTransaction.RollBack();
                return new DataResponse<UserResponse>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unknown Server Error",
                    Data = new()
                };
            }

            await dbTransaction.Commit();

            return new DataResponse<UserResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Register User",
                Data = new UserResponse
                {
                    Token = token, 
                    TokenResponse = new TokenResponse
                    {
                        Role = tokenData.Value.Role,
                        Email = tokenData.Value.Email,
                    }
                }
            };
        }
    }
}
