using Application.Shared.Responses;
using Event.Application.Models.Members;

namespace Event.Application.Interfaces
{
    public interface IMemberService
    {
        Task<DataResponse<MemberResponse>> GetMember(long memberId);

        Task<DataResponse<MemberResponse>> AddEventMember(long eventId, MemberRequest request);

        Task<BaseResponse> CancelMemberParticipation(long memberId);

        Task<DataResponse<IEnumerable<MemberResponse>>> GetMembersEvent(long eventId);

        Task<DataResponse<IEnumerable<MemberResponse>>> GetMembersEvent(long eventId, int page, int size);
    }
}
