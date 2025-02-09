namespace Authentication.Application.Model.User
{
    public class UserRequest
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
