using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Domain.Common;
using MediatR;

namespace Event.Application.Queries.Event.GetEventByName
{
    public class GetEventByNameHandler : IRequestHandler<GetEventByNameQuery, EventResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ICachService cachService;
        private readonly IMapper mapper;
        private readonly IBlobService blobService;

        public GetEventByNameHandler(
            IUnitOfWork unitOfWork,
            ICachService cachService, 
            IMapper mapper, 
            IBlobService blobService)
        {
            this.unitOfWork = unitOfWork;
            this.cachService = cachService;
            this.mapper = mapper;
            this.blobService = blobService;
        }

        public async Task<EventResponse> Handle(GetEventByNameQuery request, CancellationToken cancellationToken)
        {
            var eventCach = await cachService
                .GetData<EventResponse>("Events:" + request.Name);

            if (eventCach.IsSuccess)
            {
                return eventCach.Value;
            }

            var eventEntity = await unitOfWork.EventRepository
                .GetEventWithMembers(request.Name, cancellationToken);

            if (eventEntity is null)
            {
                throw new NotFoundApiException("Event Does Not Exist");
            }

            var eventResponse = mapper.Map<EventResponse>(eventEntity);
            eventResponse.Members = mapper
                .Map<IEnumerable<MemberResponse>>(eventEntity.Members);
            eventResponse.UrlImages = await blobService
                .DownloadBlobs(eventEntity.ImagesFolder, cancellationToken);

            var setEvent = new List<Task>()
            {
                Task.Run(() => cachService.SetData("Events:" + eventResponse.Id, eventResponse)),
                Task.Run(() => cachService.SetData("Events:" + eventResponse.Name, eventResponse))

            };
            await Task.WhenAll(setEvent);

            return eventResponse;
        }
    }
}
