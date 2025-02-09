using Application.Shared.Enums;
using Application.Shared.Responses;
using Event.Application.Interfaces;
using Event.Application.Models.Members;
using Event.Domain.Entities;
using Event.Domain.Repositories;

namespace Event.Application.Implementations
{
    public class MemberService : IMemberService
    {
        private readonly IEventMemberRepository memberRepository;
        private readonly IEventRepository eventRepository;
        private readonly ICachService cachService;

        public MemberService(
            IEventMemberRepository memberRepository, 
            IEventRepository eventRepository,
            ICachService cachService)
        {
            this.memberRepository = memberRepository;
            this.eventRepository = eventRepository;
            this.cachService = cachService;
        }

        public async Task<DataResponse<MemberResponse>> AddEventMember(long eventId, MemberRequest request)
        {
            var eventEntity = await eventRepository.GetEventWithMembers(eventId);

            if (eventEntity.IsFailure)
            {
                return new DataResponse<MemberResponse>
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Event Does Not Exist",
                    Data = new()
                };
            }

            if(eventEntity.Value.MaxMember == eventEntity.Value.Members.Count)
            {
                return new DataResponse<MemberResponse> 
                { 
                    Data = new(),
                    StatusCode = StatusCode.BadRequest,
                    Description = "Event Is Full"
                };
            }

            if(eventEntity.Value.Members.Any(x => x.Email == request.Email))
            {
                return new DataResponse<MemberResponse>
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Current Email Already Registered On This Event",
                    Data = new()
                };
            }

            var newMember = EventMember.Initialize(
                request.FirstName,
                request.SecondName,
                request.Email,
                request.BirthDate);

            if(newMember.IsFailure)
            {
                return new DataResponse<MemberResponse>
                {
                    Description = "Bad Request - " + newMember.Error,
                    StatusCode = StatusCode.BadRequest,
                    Data = new()
                };
            }

            newMember.Value.EventEntity = eventEntity.Value;

            await memberRepository.AddEventMember(newMember.Value);

            var newMemberResponse = new MemberResponse
            {
                Id = newMember.Value.Id,
                BirthDate = newMember.Value.BirthDate,
                FirstName = newMember.Value.FirstName,
                SecondName = newMember.Value.SecondName,
                Email = newMember.Value.Email,
                RegistrationDate = newMember.Value.RegistrationDate,
            };

            await cachService.SetData("Members:" + newMemberResponse.Id, newMemberResponse, 60);

            return new DataResponse<MemberResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Add New Member",
                Data = newMemberResponse
            };
        }

        public async Task<BaseResponse> CancelMemberParticipation(long memberId)
        {
            var member = await memberRepository.GetEventMember(memberId);

            if(member.IsFailure)
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Member Does Not Exist"
                };
            }

            await memberRepository.RemoveEventMember(memberId);
            await cachService.RemoveData("Members:" + member.Value.Id);

            return new BaseResponse
            {
                StatusCode = StatusCode.Ok,
                Description = "Cancel Paticipation Member"
            };
        }

        public async Task<DataResponse<MemberResponse>> GetMember(long memberId)
        {
            var cachMember = await cachService.GetData<MemberResponse>("Members:" + memberId);

            if(cachMember.IsSuccess)
            {
                return new DataResponse<MemberResponse> 
                { 
                    StatusCode = StatusCode.Ok,
                    Description = "Get Member",
                    Data = cachMember.Value
                }; 
            }

            var member = await memberRepository.GetEventMember(memberId);

            if(member.IsFailure)
            {
                return new DataResponse<MemberResponse> 
                { 
                    Data = new(),
                    StatusCode = StatusCode.NotFound,
                    Description = "Not Found Member"
                };
            }

            var memberResponse = new MemberResponse
            {
                Id = member.Value.Id,
                BirthDate = member.Value.BirthDate,
                Email = member.Value.Email,
                FirstName = member.Value.FirstName,
                SecondName = member.Value.SecondName,
                RegistrationDate = member.Value.RegistrationDate
            };

            await cachService.SetData("Members:" + memberResponse.Id, memberResponse, 60);

            return new DataResponse<MemberResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Get Member",
                Data = memberResponse
            };
        }

        public async Task<DataResponse<IEnumerable<MemberResponse>>> GetMembersEvent(long eventId)
        {
            var eventEntity = await eventRepository.GetEventWithMembers(eventId);

            if(eventEntity.IsFailure)
            {
                return new DataResponse<IEnumerable<MemberResponse>>
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Such Event Does Not Exist",
                    Data = new List<MemberResponse>()
                };
            }

            return new DataResponse<IEnumerable<MemberResponse>>
            {
                StatusCode = StatusCode.Ok,
                Description = "Get Members Event",
                Data = eventEntity.Value.Members
                    .Select(x => new MemberResponse
                    {
                        Id = x.Id,
                        FirstName= x.FirstName,
                        SecondName= x.SecondName,
                        Email = x.Email,
                        RegistrationDate = x.RegistrationDate,
                        BirthDate = x.BirthDate,
                    })
            };
        }
    }
}
