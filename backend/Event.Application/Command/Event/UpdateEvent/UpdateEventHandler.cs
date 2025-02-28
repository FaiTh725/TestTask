using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Interfaces;
using Event.Domain.Common;
using Event.Domain.Entities;
using MediatR;

namespace Event.Application.Command.Event.UpdateEvent
{
    public class UpdateEventHandler : IRequestHandler<UpdateEventCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICachService cachService;

        public UpdateEventHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICachService cachService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cachService = cachService;
        }

        public async Task Handle(UpdateEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork
                .EventRepository
                .GetEventWithMembers(request.EventId);

            if (eventEntity is null)
            {
                throw new NotFoundApiException("Event Does Not Exist");
            }

            if (eventEntity.Members.Count > request.MaxMember)
            {
                throw new ConflictApiException(
                    "New Max Members Count Less Than Members Already Registered," +
                    " Delete Members Or Set New Count");
            }

            var eventToUpdate = EventEntity.Initialize(
                eventEntity.Name,
                request.Description,
                request.Location,
                eventEntity.Category,
                request.MaxMember,
                request.TimeEvent);

            if (eventToUpdate.IsFailure)
            {
                throw new BadRequestApiException("Error With Field - " + eventToUpdate.Error);
            }

            await unitOfWork.EventRepository
                .UpdateEvent(
                eventEntity.Id, eventToUpdate.Value);

            await unitOfWork.SaveChangesAsync();

            await cachService.RemoveByPattern("Events*");
            await cachService.RemoveData("Events:" + request.EventId);
            await cachService.RemoveData("Events:" + eventEntity.Name);
        }
    }
}
