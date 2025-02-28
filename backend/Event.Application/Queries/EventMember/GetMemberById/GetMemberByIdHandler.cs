using AutoMapper;
using Event.Application.Interfaces;
using Event.Application.Models.Members;
using MediatR;
using Event.Domain.Common;
using Application.Shared.Exceptions;

namespace Event.Application.Queries.EventMember.GetMemberById
{
    public class GetMemberByIdHandler :
        IRequestHandler<GetMemberByIdQuery, MemberResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;

        public GetMemberByIdHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
        }

        public async Task<MemberResponse> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {
            var member = await unitOfWork.EventMemberRepository
                .GetEventMember(request.MemberId, cancellationToken) ?? 
                throw new NotFoundApiException("Not Found Member");
            
            var memberResponse = mapper.Map<MemberResponse>(member);

            return memberResponse;
        }
    }
}
