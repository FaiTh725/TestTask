using Event.API.Contracts.Member;
using Event.Application.Interfaces;
using Event.Application.Models.Members;
using Microsoft.AspNetCore.Mvc;

namespace Event.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventMemberController : ControllerBase
    {
        private readonly IMemberService memberService;

        public EventMemberController(
            IMemberService memberService)
        {
            this.memberService = memberService;
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddMember(CreateMemberRequest request)
        {
            var member = new MemberRequest
            {
                FirstName = request.FirstName,
                SecondName = request.SecondName,
                Email = request.Email,
                BirthDate = request.BirthDate
            };

            var response = await memberService.AddEventMember(request.EventId, member);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventMembers([FromQuery] long eventId)
        {
            var response = await memberService.GetMembersEvent(eventId);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventMember([FromQuery] long id)
        {
            var response = await memberService.GetMember(id);

            return new JsonResult(response);
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> CancelPaticipationMember(RemoveMember request)
        {
            var response = await memberService.CancelMemberParticipation(request.Id);

            return new JsonResult(response);    
        }
    }
}
