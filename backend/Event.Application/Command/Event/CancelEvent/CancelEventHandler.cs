using Application.Shared.Exceptions;
using Event.Application.Interfaces;
using Event.Domain.Common;
using MediatR;

namespace Event.Application.Command.Event.CancelEvent
{
    public class CancelEventHandler : IRequestHandler<CancelEventCommand>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachService cachService;
        private readonly IBlobService blobService;

        public CancelEventHandler(
            IUnitOfWork unitOfWork,
            ICachService cachService,
            IBlobService blobService)
        {
            this.unitOfWork = unitOfWork;
            this.cachService = cachService;
            this.blobService = blobService;
        }

        public async Task Handle(CancelEventCommand request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.
                EventRepository.GetEvent(request.EventId, cancellationToken);

            if (eventEntity is null)
            {
                throw new NotFoundApiException("Event Does Not Exist");
            }

            await unitOfWork.EventRepository
                .RemoveEvent(request.EventId, cancellationToken);

            await unitOfWork.SaveChangesAsync();

            await blobService.DeleteBlobFolder(eventEntity.ImagesFolder);
            await cachService.RemoveByPattern("Events*");

            var removeEvent = new List<Task>()
            {
                Task.Run(() => cachService.RemoveData("Events:" + eventEntity.Id)),
                Task.Run(() => cachService.RemoveData("Events:" + eventEntity.Name))

            };
            await Task.WhenAll(removeEvent);
        }
    }
}
