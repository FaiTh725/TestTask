using Application.Shared.Exceptions;
using AutoMapper;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Domain.Common;
using MediatR;

namespace Event.Application.Queries.Event.GetEventById
{
    public class GetEventByIdHandler : IRequestHandler<GetEventByIdQuery, EventResponse>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IBlobService blobService;
        private readonly ICachService cachService;
        private readonly IMapper mapper;

        public GetEventByIdHandler(
            IUnitOfWork unitOfWork, 
            IBlobService blobService,
            ICachService cachService,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.blobService = blobService;
            this.cachService = cachService;
            this.mapper = mapper;
        }

        public async Task<EventResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventCach = await cachService
                .GetData<EventResponse>("Events:" + request.Id.ToString());

            if (eventCach.IsSuccess)
            {
                return eventCach.Value;
            }

            var eventEntity = await unitOfWork.EventRepository
                .GetEventWithMembers(request.Id);

            if (eventEntity is null)
            {
                throw new NotFoundApiException("Event Does Not Exist");
            }

            var eventResponse = mapper.Map<EventResponse>(eventEntity);
            eventResponse.Members = mapper
                .Map<IEnumerable<MemberResponse>>(eventEntity.Members);
            eventResponse.UrlImages = await blobService
                .DownloadBlobs(eventEntity.ImagesFolder);

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
