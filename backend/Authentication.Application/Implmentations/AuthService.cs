using Application.Shared.Enums;
using Application.Shared.Exceptions;
using Application.Shared.Responses;
using Authentication.Application.Interfaces;
using Authentication.Application.Model.Token;
using Authentication.Application.Model.User;
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

        public AuthService(
            IUserRepository userRepository,
            IRoleRepository roleRepository, 
            IAuthTokenService tokenService,
            IHashService hashService)
        {
            this.userRepository = userRepository;
            this.roleRepository = roleRepository;
            this.tokenService = tokenService;
            this.hashService = hashService; 
        }

        // TODO: db transaction
        public async Task InitializeRoles(params string[] roles)
        {
            var existedRole = roleRepository.GetRoles();

            var removeRolesTasks = new List<Task>();

            foreach(var role in existedRole)
            {
                removeRolesTasks.Add(roleRepository.DeleteRole(role.RoleName));
            }

            await Task.WhenAll(removeRolesTasks);

            var addRolesTasks = new List<Task>();

            foreach(var role in roles)
            {
                var roleEntity = Role.Initialize(role);

                if(roleEntity.IsFailure)
                {
                    throw new ApplicationConfigurationException("", "Inialize Roles");
                }

                addRolesTasks.Add(roleRepository
                    .AddRole(roleEntity.Value));
            }

            await Task.WhenAll(addRolesTasks);
        }

        public async Task<DataResponse<UserResponse>> LoginUser(UserRequest request)
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

            if(hashService.VerifyHash(request.Password, user.Value.Password))
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
                Description = "User Has Entered",
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

            // TODO: Create Transaction

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
                return new DataResponse<UserResponse>
                {
                    StatusCode = StatusCode.InternalServerError,
                    Description = "Unknown Server Error",
                    Data = new()
                };
            }

            return new DataResponse<UserResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Use Has Registered",
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
