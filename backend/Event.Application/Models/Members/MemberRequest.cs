namespace Event.Application.Models.Members
{
    public class MemberRequest
    {
        public string FirstName { get; set; } = string.Empty;

        public string SecondName { get; set; } = string.Empty;

        public string Email {  get; set; } = string.Empty;

        public DateTime? BirthDate { get; set; }
    }
}
