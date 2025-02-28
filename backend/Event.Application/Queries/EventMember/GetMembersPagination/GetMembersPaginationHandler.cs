using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Models.Members;
using Event.Domain.Common;
using MediatR;

namespace Event.Application.Queries.EventMember.GetMembersPagination
{
    public class GetMembersPaginationHandler :
        IRequestHandler<GetMembersPaginationQuery, IEnumerable<MemberResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetMembersPaginationHandler(
            IUnitOfWork unitOfWork, IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<MemberResponse>> Handle(GetMembersPaginationQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.EventRepository
                .GetEventWithMembers(request.EventId) ?? 
                throw new NotFoundApiException("Such Event Does Not Exist");

            var eventsPagination = unitOfWork.EventMemberRepository
                .GetEventMembers(request.EventId, request.Page, request.Size);

            var eventMembers = mapper.Map<IEnumerable<MemberResponse>>(eventsPagination);

            return eventMembers;
        }
    }
}
