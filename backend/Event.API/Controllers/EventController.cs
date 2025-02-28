using Event.API.Contracts.Event;
using Event.Application.Command.Event.CancelEvent;
using Event.Application.Command.Event.CreateEvent;
using Event.Application.Command.Event.UpdateEvent;
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
        public async Task<IActionResult> GetEvents(
            CancellationToken token)
        {
            var events = await mediator.Send(new GetEventsQuery(), token);

            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsPagination(
            int page, 
            int size,
            CancellationToken token)
        {
            var events = await mediator.Send(
                new GetEventPaginationQuery
                {
                    Page = page,
                    Size = size
                }, 
                token);

            return Ok(events);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventById(
            long id,
            CancellationToken token)
        {

            var eventEntity = await mediator.Send(
                new GetEventByIdQuery
                {
                    Id = id
                },
                token);

            return Ok(eventEntity);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventByName(
            string name,
            CancellationToken token)
        {
            var eventEntity = await mediator.Send(
                new GetEventByNameQuery
                {
                    Name = name
                }, 
                token);

            return Ok(eventEntity);
        }

        [HttpPatch("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateEvent(
            UpdateEventCommand request, 
            CancellationToken token)
        {
            await mediator.Send(request, token);

            return Ok();
        }

        [HttpDelete("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CancelEvent(
            CancelEventCommand request,
            CancellationToken token)
        {
            await mediator.Send(request, token);

            return Ok();
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventsByQuery(
            [FromQuery]GetEventsByQueryQuery request,
            CancellationToken token)
        {
            var events = await mediator.Send(request, token);

            return Ok(events);
        }

        [HttpPost("[action]")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateEvent(
            CreateEventRequest request,
            CancellationToken token)
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

            var eventId = await mediator.Send(eventRequest, token);

            var eventEntity = await mediator.Send(
                new GetEventByIdQuery 
                { 
                    Id = eventId
                }, 
                token);

            return Ok(eventEntity);
        }
    }
}
