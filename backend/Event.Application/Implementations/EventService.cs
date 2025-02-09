﻿using Application.Shared.Enums;
using Application.Shared.Responses;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Application.Specifications;
using Event.Application.Specifications.Event;
using Event.Domain.Common.Specifications;
using Event.Domain.Entities;
using Event.Domain.Repositories;

namespace Event.Application.Implementations
{
    public class EventService : IEventService
    {
        private readonly IEventRepository eventRepository;
        private readonly IBlobService blobService;
        private readonly ICachService cachService;

        public EventService(
            IEventRepository eventRepository,
            IBlobService blobService,
            ICachService cachService)
        {
            this.eventRepository = eventRepository;
            this.blobService = blobService;
            this.cachService = cachService;
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

            await blobService.DeleteBlobFolder(eventEntity.Value.ImagesFolder);
            await cachService.RemoveData("Events:" + eventEntity.Value.Id);
            await cachService.RemoveData("Events:" + eventEntity.Value.Name);

            return new BaseResponse
            {
                StatusCode = StatusCode.Ok,
                Description = "Cancel Event"
            };
        }

        public async Task<DataResponse<EventResponse>> GetEvent(long eventId)
        {
            var eventCach = await cachService.GetData<EventResponse>("Events:" + eventId.ToString());

            if (eventCach.IsSuccess)
            {
                return new DataResponse<EventResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get Event",
                    Data = new EventResponse
                    {
                        Id = eventCach.Value.Id,
                        Description = eventCach.Value.Description,
                        Category = eventCach.Value.Category,
                        Location = eventCach.Value.Location,
                        MaxMembers = eventCach.Value.MaxMembers,
                        Name = eventCach.Value.Name,
                        TimeEvent = eventCach.Value.TimeEvent,
                        UrlImages = eventCach.Value.UrlImages
                    }
                };
            }

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

            var eventResponse = new EventResponse
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
            };

            await cachService.SetData("Events:" + eventResponse.Id, eventResponse);
            await cachService.SetData("Events:" + eventResponse.Name, eventResponse);

            return new DataResponse<EventResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Get Event",
                Data = eventResponse
            };
        }

        public async Task<DataResponse<EventResponse>> GetEvent(string eventName)
        {
            var eventCach = await cachService.GetData<EventResponse>("Events:" + eventName);

            if (eventCach.IsSuccess)
            {
                return new DataResponse<EventResponse>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get Event",
                    Data = new EventResponse
                    {
                        Id = eventCach.Value.Id,
                        Description = eventCach.Value.Description,
                        Category = eventCach.Value.Category,
                        Location = eventCach.Value.Location,
                        MaxMembers = eventCach.Value.MaxMembers,
                        Name = eventCach.Value.Name,
                        TimeEvent = eventCach.Value.TimeEvent,
                        UrlImages = eventCach.Value.UrlImages
                    }
                };
            }

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

            var eventResponse = new EventResponse
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
            };

            await cachService.SetData("Events:" + eventResponse.Id, eventResponse);
            await cachService.SetData("Events:" + eventResponse.Name, eventResponse);

            return new DataResponse<EventResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Get Event",
                Data = eventResponse
            };
        }

        public async Task<DataResponse<IEnumerable<EventResponse>>> GetEvents()
        {
            var cachEvents = await cachService.GetData<IEnumerable<EventResponse>>("Events");

            if (cachEvents.IsSuccess)
            {
                return new DataResponse<IEnumerable<EventResponse>>
                {
                    StatusCode = StatusCode.Ok,
                    Description = "Get Events",
                    Data = cachEvents.Value
                };
            }

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

            await cachService.SetData("Events", eventsResponse);

            return new DataResponse<IEnumerable<EventResponse>>
            {
                StatusCode = StatusCode.Ok,
                Description = "Get Events",
                Data = eventsResponse
            };
        }

        public async Task<DataResponse<IEnumerable<EventResponse>>> GetEvents(string? location, string? category, DateTime? eventTime)
        {
            Specification<EventEntity> specification = new IncludeMemberSpecification();

            if(!string.IsNullOrEmpty(location))
            {
                var locationSpecification = new LocationSpecification(location);

                specification = new AndSpecification<EventEntity>(locationSpecification, specification);
            }

            if(!string.IsNullOrEmpty(category))
            {
                var categorySpecification = new CategorySpecification(category);

                specification = new AndSpecification<EventEntity>(categorySpecification, specification);
            }

            if (eventTime.HasValue)
            {
                var dateSpecification = new DateSpecification(eventTime.Value);

                specification = new AndSpecification<EventEntity>(dateSpecification, specification);
            }

            var events = eventRepository
                .GetEvents(specification)
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
                }).ToList(),
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

            var newEvent = new EventResponse
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
            };

            await cachService.SetData("Events:" + newEvent.Id, newEvent);
            await cachService.SetData("Events:" + newEvent.Name, newEvent);

            return new DataResponse<EventResponse>
            {
                StatusCode = StatusCode.Ok,
                Description = "Create New Event",
                Data = newEvent
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

            await cachService.RemoveData("Events:" + request.EventId);
            await cachService.RemoveData("Events:" + eventEntity.Value.Name);

            return new BaseResponse
            {
                StatusCode = StatusCode.Ok,
                Description = "Update Event"
            };
        }
    }
}
