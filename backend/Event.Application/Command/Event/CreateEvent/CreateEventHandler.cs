using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Domain.Common;
using Event.Domain.Entities;
using MediatR;

namespace Event.Application.Command.Event.CreateEvent
{
    public class CreateEventHandler : IRequestHandler<CreateEventCommand, long>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachService cachService;
        private readonly IBlobService blobService;
        private readonly IMapper mapper;

        public CreateEventHandler(
            IUnitOfWork unitOfWork,
            ICachService cachService,
            IBlobService blobService,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.cachService = cachService;
            this.blobService = blobService;
            this.mapper = mapper;
        }


        public async Task<long> Handle(CreateEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.EventRepository
                .GetEvent(request.Name, cancellationToken);

            if (eventEntity is not null)
            {
                throw new ConflictApiException("Event With The Same Name Already Exist");
            }

            var newEntity = EventEntity.Initialize(
                request.Name,
                request.Description,
                request.Location,
                request.Category,
                request.MaxMember,
                request.TimeEvent);

            if (newEntity.IsFailure)
            {
                throw new BadRequestApiException("Error Initialize Event Fields - " +
                    newEntity.Error);
            }

            var entityFromDb = await unitOfWork
                .EventRepository
                .AddEvent(newEntity.Value, cancellationToken);

            await unitOfWork.SaveChangesAsync();

            var tasks = new List<Task<string>>();

            // upload images to blob storage
            foreach (var blob in request.Images)
            {
                tasks.Add(blobService.UploadBlob(
                blob.Content,
                blob.Name,
                    blob.ContentType,
                    entityFromDb.ImagesFolder));
            }

            var urlImages = await Task.WhenAll(tasks);

            var newEvent = mapper.Map<EventResponse>(entityFromDb);
            newEvent.Members = Enumerable.Empty<MemberResponse>();
            newEvent.UrlImages = urlImages;

            await cachService.RemoveByPattern("Events*");
            var setEvent = new List<Task>()
            {
                Task.Run(() => cachService.SetData("Events:" + newEvent.Id, newEvent)),
                Task.Run(() => cachService.SetData("Events:" + newEvent.Name, newEvent))

            };
            await Task.WhenAll(setEvent);

            return newEvent.Id;
        }
    }
}
