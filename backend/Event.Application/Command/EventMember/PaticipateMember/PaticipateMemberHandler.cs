using Application.Shared.Exceptions;
using Event.Domain.Common;
using MediatR;
using EventMemberEntity = Event.Domain.Entities.EventMember; 

namespace Event.Application.Command.EventMember.PaticipateMember
{
    public class PaticipateMemberHandler :
        IRequestHandler<PaticipateMemberCommand, long>
    {
        private readonly IUnitOfWork unitOfWork;

        public PaticipateMemberHandler(
            IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        public async Task<long> Handle(PaticipateMemberCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.EventRepository
                .GetEventWithMembers(request.EventId, cancellationToken) ?? 
                throw new BadRequestApiException("Event Does Not Exist");
            
            if (eventEntity.MaxMember == eventEntity.Members.Count)
            {
                throw new ConflictApiException("Event Is Full");
            }

            if (eventEntity.Members.Any(x => x.Email == request.Email))
            {
                throw new ConflictApiException("Current Email Already Registered On This Event");
            }

            var newMember = EventMemberEntity.Initialize(
                request.FirstName,
                request.SecondName,
                request.Email,
                request.BirthDate);

            if (newMember.IsFailure)
            {
                throw new BadRequestApiException("Bad Request - " + newMember.Error);
            }

            newMember.Value.EventEntity = eventEntity;
            await unitOfWork.EventMemberRepository
                .AddEventMember(newMember.Value, cancellationToken);

            await unitOfWork.SaveChangesAsync();

            return newMember.Value.Id;
        }
    }
}
