using CSharpFunctionalExtensions;

namespace Event.Domain.Entities
{
    public class EventEntity
    {
        public long Id { get; }

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        // I would create a value object
        // table would match the xNF form
        public string Location { get; private set; } = string.Empty;

        // Like with Location
        public string Category { get; init; } = string.Empty;

        public DateTime TimeEvent { get; private set; }

        public int MaxMember { get; private set; }

        public string ImagesFolder
        {
            get => $"Event-{Id}";
        }

        public List<EventMember> Members { get; init; } = new List<EventMember>();

        private EventEntity(
            string name,
            string description,
            string location,
            string category,
            int maxMember)
        {
            Name = name;
            Description = description;
            Location = location;
            Category = category;
            MaxMember = maxMember;

            TimeEvent = DateTime.UtcNow;
        }

        public static Result<EventEntity> Initialize(
            string name,
            string description,
            string location,
            string category,
            int maxMember)
        {
            if(name is null ||
                description is null ||
                location is null ||
                category is null)
            {
                return Result.Failure<EventEntity>("String value is null");
            }

            if(string.IsNullOrEmpty(name) ||
                string.IsNullOrEmpty(description) ||
                string.IsNullOrEmpty(location) ||
                string.IsNullOrEmpty(category))
            {
                return Result.Failure<EventEntity>("Name, Description, Location and Category " +
                    "is empty");
            }

            if(maxMember < 0)
            {
                return Result.Failure<EventEntity>("Max member count less than zero");
            }

            return Result.Success(
                new EventEntity(
                    name,
                    description,
                    location,
                    category,
                    maxMember));
        }
    }
}
