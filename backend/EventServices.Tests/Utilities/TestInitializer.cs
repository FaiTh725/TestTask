using Event.Domain.Entities;
using System.Reflection;

namespace EventServices.Tests.Utilities
{
    public static class TestInitializer
    {
        public static EventMember GetTestEventMember(
            long id,
            string firstName,
            string secondName,
            string email,
            DateTime birthday)
        {
            var memberResult = EventMember.Initialize(
                firstName,
                secondName,
                email,
                birthday);

            if(memberResult.IsFailure)
            {
                throw new InvalidDataException("Error Initialize Test Member");
            }

            var member = memberResult.Value;

            var idField = typeof(EventMember).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);

            if (idField is not null )
            {
                idField.SetValue(member, id);
            }

            return member;
        }

        public static EventEntity GetTestEvent(
            long id,
            string name,
            string description,
            string location,
            string category,
            int maxMember,
            DateTime timeEvent,
            IEnumerable<EventMember> members)
        {
            var eventEntity = EventEntity.Initialize(
                name,
                description,
                location,
                category,
                maxMember,
                timeEvent);

            if(eventEntity.IsFailure)
            {
                throw new InvalidDataException("Error Initalize Test Event");
            }

            eventEntity.Value.Members.AddRange(members);

            var eventObj = eventEntity.Value;

            var idField = typeof(EventEntity).GetField("<Id>k__BackingField", BindingFlags.Instance | BindingFlags.NonPublic);
        
            if (idField is not null)
            {
                idField.SetValue(eventObj, id);
            }

            return eventObj;
        }
    }
}
