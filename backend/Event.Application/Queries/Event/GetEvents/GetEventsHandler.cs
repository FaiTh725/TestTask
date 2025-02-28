using AutoMapper;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Domain.Common;
using MediatR;

namespace Event.Application.Queries.Event.GetEvents
{
    public class GetEventsHandler : IRequestHandler<GetEventsQuery, IEnumerable<EventResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly ICachService cachService;
        private readonly IBlobService blobService;

        public GetEventsHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            ICachService cachService,
            IBlobService blobService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.cachService = cachService;
            this.blobService = blobService;
        }

        public async Task<IEnumerable<EventResponse>> Handle(GetEventsQuery request, CancellationToken cancellationToken)
        {
            var cachEvents = await cachService
                .GetData<IEnumerable<EventResponse>>("Events");

            if (cachEvents.IsSuccess)
            {
                return cachEvents.Value;
            }
            var events = unitOfWork.EventRepository
                .GetEventsWithMembers();

            var eventTasks = events.Select(async x =>
            {
                var eventResponse = mapper.Map<EventResponse>(x);
                eventResponse.Members = mapper
                    .Map<IEnumerable<MemberResponse>>(x.Members);
                eventResponse.UrlImages = await blobService
                    .DownloadBlobs(x.ImagesFolder);

                return eventResponse;
            }).ToList();

            var eventsResponse = await Task.WhenAll(eventTasks);

            await cachService.SetData("Events", eventsResponse);

            return eventsResponse;
        }
    }
}
