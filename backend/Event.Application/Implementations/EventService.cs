using Application.Shared.Enums;
using Application.Shared.Responses;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Domain.Entities;
using Event.Domain.Repositories;

namespace Event.Application.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;
        private readonly IBlobService blobService;

        public EventService(
            IEventRepository eventRepository,
            IBlobService blobService)
        {
            this.eventRepository = eventRepository;
            this.blobService = blobService;
        }

        public async Task<BaseResponse> CancelEvent(long eventId)
        {
            var eventEntity = await eventRepository.GetEvent(eventId);

            if(eventEntity.IsFailure)
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Event Does Not Exist"
                };
            }

            await eventRepository.RemoveEvent(eventId);

            return new BaseResponse
            {
                StatusCode = StatusCode.Ok,
                Description = "Cancel Event"
            };
        }

        // TODO: create universal method to get event by propperties
        public async Task<DataResponse<EventResponse>> GetEvent(long eventId)
        {
            var eventEntity = await eventRepository.GetEvent(eventId);

            if (eventEntity.IsFailure)
            {
                return new DataResponse<EventResponse>
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Event Does Not Exist",
                    Data = new()
                };
            }

            return new DataResponse<EventResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Get Event",
                Data = new EventResponse
                { 
                    Id = eventEntity.Value.Id,
                    Description = eventEntity.Value.Description,
                    Category = eventEntity.Value.Category,
                    Location = eventEntity.Value.Location,
                    MaxMembers = eventEntity.Value.MaxMember,
                    Name = eventEntity.Value.Name,
                    TimeEvent = eventEntity.Value.TimeEvent,
                    UrlImages = await blobService
                        .DownloadBlobs(eventEntity.Value.ImagesFolder)
                }
            };
        }

        public async Task<DataResponse<EventResponse>> GetEvent(string eventName)
        {
            var eventEntity = await eventRepository.GetEvent(eventName);

            if (eventEntity.IsFailure)
            {
                return new DataResponse<EventResponse>
                {
                    StatusCode = StatusCode.NotFound,
                    Description = "Event Does Not Exist",
                    Data = new()
                };
            }

            return new DataResponse<EventResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Get Event",
                Data = new EventResponse
                {
                    Id = eventEntity.Value.Id,
                    Description = eventEntity.Value.Description,
                    Category = eventEntity.Value.Category,
                    Location = eventEntity.Value.Location,
                    MaxMembers = eventEntity.Value.MaxMember,
                    Name = eventEntity.Value.Name,
                    TimeEvent = eventEntity.Value.TimeEvent,
                    UrlImages = await blobService
                        .DownloadBlobs(eventEntity.Value.ImagesFolder)
                }
            };
        }

        public async Task<DataResponse<IEnumerable<EventResponse>>> GetEvents()
        {
            var events = eventRepository.GetEventsWithMembers()
                .ToList();

            var eventTasks = events.Select(async x => new EventResponse
            {
                Id = x.Id,
                Name = x.Name,
                Description = x.Description,
                Category = x.Category,
                Location = x.Location,
                MaxMembers = x.MaxMember,
                TimeEvent = x.TimeEvent,
                Members = x.Members.Select(y => new MemberResponse
                {
                    Id = y.Id,
                    FirstName = y.FirstName,
                    SecondName = y.SecondName,
                    Email = y.Email,
                    BirthDate = y.BirthDate,
                    RegistrationDate = y.RegistrationDate
                }).AsEnumerable(),
                UrlImages = await blobService.DownloadBlobs(x.ImagesFolder)
            }).ToList();

            var eventsResponse = await Task.WhenAll(eventTasks);

            return new DataResponse<IEnumerable<EventResponse>>
            {
                StatusCode = StatusCode.Ok,
                Description = "Get Events",
                Data = eventsResponse
            };
        }

        public async Task<DataResponse<EventResponse>> RegistrNewEvent(EventRequest request)
        {
            var eventEntity = await eventRepository.GetEvent(request.Name);

            if(eventEntity.IsSuccess)
            {
                return new DataResponse<EventResponse>
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Event With The Same Name Already Exist",
                    Data = new()
                };
            }

            var newEntity = EventEntity.Initialize(
                request.Name,
                request.Description,
                request.Location,
                request.Category,
                request.MaxMember,
                request.TimeEvent);

            if(newEntity.IsFailure)
            {
                return new DataResponse<EventResponse>
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Error Initialize Event Fields - " +
                    newEntity.Error,
                    Data = new()
                };
            }

            var entityFromDb = await eventRepository
                .AddEvent(newEntity.Value);

            var tasks = new List<Task<string>>();

            // upload images to blob storage
            foreach(var blob in request.Images)
            {
                tasks.Add(blobService.UploadBlob(
                    blob.Content,
                    $"{entityFromDb.Value.ImagesFolder}/{entityFromDb.Value.Name}",
                    blob.ContentType));
            }

            var urlImages = await Task.WhenAll(tasks);

            return new DataResponse<EventResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Create New Event",
                Data = new EventResponse
                {
                    Id = entityFromDb.Value.Id,
                    Description = entityFromDb.Value.Description,
                    Category = entityFromDb.Value.Category,
                    Location = entityFromDb.Value.Location,
                    Name = entityFromDb.Value.Name,
                    MaxMembers = entityFromDb.Value.MaxMember,
                    TimeEvent = entityFromDb.Value.TimeEvent,
                    UrlImages = urlImages,
                    Members = Enumerable.Empty<MemberResponse>()
                }
            };
                                    
        }

        public async Task<BaseResponse> UpdateEvent(UpdateEventRequest request)
        {
            var eventEntity = await eventRepository
                .GetEventWithMembers(request.EventId);

            if (eventEntity.IsFailure)
            {
                return new BaseResponse
                { 
                    StatusCode = StatusCode.NotFound,
                    Description = "Event Does Not Exist"
                };
            }

            if(eventEntity.Value.Members.Count > request.MaxMember)
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "New Max Members Count Less Than Members Already Registered," +
                    " Delete Members Or Set New Count" 
                };
            }

            var eventToUpdate = EventEntity.Initialize(
                eventEntity.Value.Name,
                request.Description,
                request.Location,
                eventEntity.Value.Category,
                request.MaxMember,
                request.TimeEvent);

            if(eventToUpdate.IsFailure)
            {
                return new BaseResponse
                {
                    StatusCode = StatusCode.BadRequest,
                    Description = "Error With Field - " + eventToUpdate.Error
                };
            }

            await eventRepository.UpdateEvent(
                eventEntity.Value.Id, eventToUpdate.Value);

            return new BaseResponse
            {
                StatusCode = StatusCode.Ok,
                Description = "Update Event"
            };
        }
    }
}
