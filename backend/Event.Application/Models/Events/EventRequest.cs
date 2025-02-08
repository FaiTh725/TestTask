using Event.Application.Models.Files;

namespace Event.Application.Models.Events
{
    public class EventRequest
    {
        public string Name { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public DateTime TimeEvent { get; set; }

        public int MaxMember {  get; set; }

        public List<FileRequest> Images { get; set; } = [];
    }
}
