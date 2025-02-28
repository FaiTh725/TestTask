using AutoMapper;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Domain.Common;
using MediatR;

namespace Event.Application.Queries.Event.GetEventsPagination
{
    public class GetEventPaginationHandler :
        IRequestHandler<GetEventPaginationQuery, IEnumerable<EventResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IBlobService blobService;

        public GetEventPaginationHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBlobService blobService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.blobService = blobService;
        }

        public async Task<IEnumerable<EventResponse>> Handle(GetEventPaginationQuery request, CancellationToken cancellationToken)
        {
            var eventsPagination = unitOfWork.EventRepository
                .GetEventsWithMembers(request.Page, request.Size);

            var eventTasks = eventsPagination.Select(async x =>
            {
                var eventResponse = mapper.Map<EventResponse>(x);
                eventResponse.Members = mapper
                    .Map<IEnumerable<MemberResponse>>(x.Members);
                eventResponse.UrlImages = await blobService
                .DownloadBlobs(x.ImagesFolder, cancellationToken);
                return eventResponse;
            }).ToList();

            var eventsResponse = await Task.WhenAll(eventTasks);

            return eventsResponse;
        }
    }
}
