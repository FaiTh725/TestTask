using Application.Shared.Responses;
using Authentication.Application.Model.Token;
using Authentication.Application.Model.User;

namespace Authentication.Application.Interfaces
{
    public interface IAuthService
    {
        Task<DataResponse<UserResponse>> LoginUser(UserRequest request);

        Task<DataResponse<UserResponse>> RegisterUser(UserRequest request);
    }
}
