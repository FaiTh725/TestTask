using CSharpFunctionalExtensions;
using System.Text.RegularExpressions;

namespace Event.Domain.Entities
{
    public class EventMember
    {
        public long Id { get; }

        public string FirstName { get; private set; } = string.Empty;

        public string SecondName { get; private set; } = string.Empty;

        public string Email { get; init; } = string.Empty;

        public DateTime? BirthDate { get; private set; }

        public DateTime RegistrationDate { get; init; }
        
        public EventEntity? EventEntity { get; set; }

        public EventMember() { }

        private EventMember(
            string firstName,
            string secondName,
            string email,
            DateTime? birthDate = null)
        {
            FirstName = firstName;
            SecondName = secondName;
            Email = email;
            BirthDate = birthDate;

            RegistrationDate = DateTime.UtcNow;
        }

        public static Result<EventMember> Initialize(
            string firstName,
            string secondName,
            string email,
            DateTime? birthDate = null)
        {
            if(firstName is null ||
                secondName is null ||
                email is null)
            {
                return Result.Failure<EventMember>("String value is null");
            }

            if(string.IsNullOrEmpty(firstName) ||
                string.IsNullOrEmpty(secondName))
            {
                return Result.Failure<EventMember>("FirstName, SecondName is empty");
            }

            var emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$");

            if(!emailRegex.IsMatch(email))
            {
                return Result.Failure<EventMember>("Email address is invalid");
            }

            if(birthDate > DateTime.UtcNow )
            {
                return Result.Failure<EventMember>("BirthDate is greater than current");
            }

            return Result.Success(new EventMember(
                firstName,
                secondName,
                email,
                birthDate));
        }

    }
}
