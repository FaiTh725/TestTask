using Event.Application.Command.EventMember.CancelPaticipateMember;
using Event.Application.Command.EventMember.PaticipateMember;
using Event.Application.Queries.Event.GetEventById;
using Event.Application.Queries.EventMember.GetMemberById;
using Event.Application.Queries.EventMember.GetMembersByEventId;
using Event.Application.Queries.EventMember.GetMembersPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventMemberController : ControllerBase
    {
        private readonly IMediator mediator;

        public EventMemberController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpPost("[action]")]
        [Authorize]
        public async Task<IActionResult> AddMember(
            PaticipateMemberCommand request, 
            CancellationToken token)
        {
            var memberId = await mediator.Send(request, token);

            var member = await mediator.Send(new GetMemberByIdQuery 
            { 
                MemberId = memberId
            }, 
            token);

            return Ok(member);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventMembers(
            [FromQuery]GetMembersByEventIdQuery request,
            CancellationToken token)
        {
            var members = await mediator.Send(request, token);
            

            return Ok(members);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventMembersPagination(
            [FromQuery]GetMembersPaginationQuery request,
            CancellationToken token)
        {
            var members = await mediator.Send(request, token);
            
            return Ok(members);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventMember(
            long id, 
            CancellationToken token)
        {
            var member = await mediator.Send(
                new GetEventByIdQuery
                {
                    Id = id
                }, 
                token);

            return Ok(member);
        }

        [HttpPatch("[action]")]
        [Authorize]
        public async Task<IActionResult> CancelPaticipationMember(CancelPaticipateMemberCommand request)
        {
            await mediator.Send(request);

            return Ok();    
        }
    }
}
