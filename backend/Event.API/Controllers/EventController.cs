using Event.API.Contracts.Event;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Files;
using Microsoft.AspNetCore.Mvc;

namespace Event.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EventController : ControllerBase
    {
        private readonly IEventService eventService;

        public EventController(
            IEventService eventService)
        {
            this.eventService = eventService;   
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEvents()
        {
            var response = await eventService.GetEvents();

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventById([FromQuery] long id)
        {
            var response = await eventService.GetEvent(id);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventByName([FromQuery] string name)
        {
            var response = await eventService.GetEvent(name);

            return new JsonResult(response);
        }

        [HttpPatch("[action]")]
        public async Task<IActionResult> UpdateEvent(UpdateEventRequest request)
        {
            var response = await eventService.UpdateEvent(request);

            return new JsonResult(response);
        }

        [HttpDelete("[action]")]
        public async Task<IActionResult> CancelEvent(long id)
        {
            var response = await eventService.CancelEvent(id);

            return new JsonResult(response);
        }

        [HttpGet("[action]")]
        public async Task<IActionResult> GetEventByQuery()
        {
            throw new NotImplementedException();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> CreateEvent(CreateEventRequest request)
        {
            var eventRequest = new EventRequest
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

            var response = await eventService.RegistrNewEvent(eventRequest);

            return new JsonResult(response);
        }
    }
}
