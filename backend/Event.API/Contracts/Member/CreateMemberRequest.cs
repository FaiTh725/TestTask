namespace Event.API.Contracts.Member
{
    public class CreateMemberRequest
    {
        public long EventId { get; set; }

        public string FirstName { get; set; } = string.Empty;

        public string SecondName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }
    }
}
