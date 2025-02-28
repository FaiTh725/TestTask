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
        private readonly ICachService cachService;
        private readonly IMapper mapper;

        public GetMemberByIdHandler(
            IUnitOfWork unitOfWork,
            ICachService cachService,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.cachService = cachService;
            this.mapper = mapper;
        }

        // TODO: Implement Mediatr pipeline bahavior for caching
        public async Task<MemberResponse> Handle(GetMemberByIdQuery request, CancellationToken cancellationToken)
        {
            var cachMember = await cachService
                .GetData<MemberResponse>("Members:" + request.MemberId);

            if (cachMember.IsSuccess)
            {
                return cachMember.Value;
            }

            var member = await unitOfWork.EventMemberRepository
                .GetEventMember(request.MemberId, cancellationToken) ?? 
                throw new NotFoundApiException("Not Found Member");
            
            var memberResponse = mapper.Map<MemberResponse>(member);

            await cachService.SetData("Members:" + memberResponse.Id, memberResponse, 60);

            return memberResponse;
        }
    }
}
