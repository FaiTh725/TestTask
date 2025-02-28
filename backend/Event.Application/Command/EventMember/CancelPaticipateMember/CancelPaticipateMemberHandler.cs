using Application.Shared.Exceptions;
using Event.Application.Interfaces;
using Event.Domain.Common;
using MediatR;

namespace Event.Application.Command.EventMember.CancelPaticipateMember
{
    public class CancelPaticipateMemberHandler : IRequestHandler<CancelPaticipateMemberCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachService cachService;

        public CancelPaticipateMemberHandler(
            IUnitOfWork unitOfWork,
            ICachService cachService)
        {
            this.unitOfWork = unitOfWork;
            this.cachService = cachService;
        }

        public async Task Handle(CancelPaticipateMemberCommand request, CancellationToken cancellationToken)
        {
            var member = await unitOfWork.EventMemberRepository
                .GetEventMember(request.MemberId) ??
                throw new NotFoundApiException("Member Does Not Exist");

            await unitOfWork.EventMemberRepository
                .RemoveEventMember(request.MemberId);
            await cachService.RemoveData("Members:" + member.Id);
        }
    }
}
