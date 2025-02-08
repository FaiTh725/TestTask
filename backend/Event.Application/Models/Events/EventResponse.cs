using Event.Application.Models.Members;

namespace Event.Application.Models.Events
{
    public class EventResponse
    {
        public long Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Location {  get; set; } = string.Empty;

        public string Category {  get; set; } = string.Empty;

        public DateTime TimeEvent { get; set; }
        
        public int MaxMembers {  get; set; }
        
        public IEnumerable<string> UrlImages { get; set; } = [];

        public IEnumerable<MemberResponse> Members { get; set; } = [];
    }
}
