using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace Authentication.Domain.Entities
{
    public class User
    {
        public const int PASSWORD_MAX_LENGTH = 20;
        public const int PASSWORD_MIN_LENGTH = 5;

        public long Id { get; }

        public string Email { get; } = string.Empty;

        public string Password { get; } = string.Empty;

        public Role Role { get; }

        // For EF
        public User() { }

        private User(
            string email, 
            string password, 
            Role role)
        {
            Email = email;
            Password = password;
            Role = role;
        }

        public static Result<User> Initialize(
            string email,
            string password,
            Role role)
        {
            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if(!emailRegex.IsMatch(email))
            {
                return Result.Failure<User>("Email Is Null Or " +
                    "Invalid Signature");
            }

            if(password is null ||
                password.Length < PASSWORD_MIN_LENGTH ||
                password.Length > PASSWORD_MAX_LENGTH)
            {
                return Result.Failure<User>("Password Is Null Or " +
                    $"Out Of Length from {PASSWORD_MIN_LENGTH} {PASSWORD_MAX_LENGTH}");
            }

            Regex passwordRegex = new Regex(@"^(?=.*[A-Za-z])(?=.*\d).+$");

            if(!passwordRegex.IsMatch(password))
            {
                return Result.Failure<User>("Password does not contains one letter " +
                    "and one number");
            }

            if (role is null)
            {
                return Result.Failure<User>("Role Is Null");
            }

            return Result.Success(new User(
                email,
                password,
                role));
        }
    }
}
