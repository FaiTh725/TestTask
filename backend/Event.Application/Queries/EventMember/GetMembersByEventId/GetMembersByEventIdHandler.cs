using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Models.Members;
using Event.Domain.Common;
using MediatR;

namespace Event.Application.Queries.EventMember.GetMembersByEventId
{
    public class GetMembersByEventIdHandler :
        IRequestHandler<GetMembersByEventIdQuery, IEnumerable<MemberResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetMembersByEventIdHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MemberResponse>> Handle(GetMembersByEventIdQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.EventRepository
                .GetEventWithMembers(request.EventId, cancellationToken) ?? 
                throw new NotFoundApiException("Such Event Does Not Exist");
            
            var eventMembers = mapper.Map<IEnumerable<MemberResponse>>(eventEntity.Members);

            return eventMembers;
        }
    }
}
