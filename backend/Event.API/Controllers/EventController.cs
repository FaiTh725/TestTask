using Event.API.Contracts.Event;
using Event.Application.Command.Event.CancelEvent;
using Event.Application.Command.Event.CreateEvent;
using Event.Application.Command.Event.UpdateEvent;
using Event.Application.Interfaces;
using Event.Application.Models.Files;
using Event.Application.Queries.Event.GetEventById;
using Event.Application.Queries.Event.GetEventByName;
using Event.Application.Queries.Event.GetEvents;
using Event.Application.Queries.Event.GetEventsByQuery;
using Event.Application.Queries.Event.GetEventsPagination;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Event.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IMediator mediator;

        public EventController(
            IMediator mediator)
        {
            this.mediator = mediator;
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEvents()
        {
            var events = await mediator.Send(new GetEventsQuery());

            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsPagination([FromQuery]GetEventPaginationQuery request)
        {
            var events = await mediator.Send(request);

            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventById([FromQuery] GetEventByIdQuery request)
        {
            var eventEntity = await mediator.Send(request);

            return Ok(eventEntity);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventByName([FromQuery]GetEventByNameQuery request)
        {
            var eventEntity = await mediator.Send(request);

            return Ok(eventEntity);
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent(UpdateEventCommand request)
        {
            await mediator.Send(request);

            return Ok();
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CancelEvent(CancelEventCommand request)
        {
            await mediator.Send(request);

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsByQuery([FromQuery] GetEventsByQueryQuery request)
        {
            var events = await mediator.Send(request);

            return Ok(events);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent(CreateEventRequest request)
        {
            var eventRequest = new CreateEventCommand
            {
                Name = request.Name,
                Description = request.Description,
                Location = request.Location,
                Category = request.Category,
                TimeEvent = request.TimeEvent,
                MaxMember = request.MaxMember,
                Images = request.Files == null ? 
                    new List<FileRequest>() :
                    request.Files.Select(x => new FileRequest 
                    { 
                        Content = x.OpenReadStream(),
                        ContentType = x.ContentType,
                        Name = x.Name,
                    }).ToList()
            };

            var eventId = await mediator.Send(eventRequest);

            var eventEntity = await mediator.Send(new GetEventByIdQuery { Id = eventId});

            return Ok(eventEntity);
        }
    }
}
