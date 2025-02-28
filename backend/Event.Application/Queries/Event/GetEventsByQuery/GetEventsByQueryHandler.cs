using AutoMapper;
using Event.Application.Interfaces;
using Event.Application.Models.Events;
using Event.Application.Models.Members;
using Event.Application.Specifications.Event;
using Event.Application.Specifications;
using Event.Domain.Common.Specifications;
using Event.Domain.Entities;
using MediatR;
using Event.Domain.Common;

namespace Event.Application.Queries.Event.GetEventsByQuery
{
    public class GetEventsByQueryHandler :
        IRequestHandler<GetEventsByQueryQuery, IEnumerable<EventResponse>>
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IBlobService blobService;

        public GetEventsByQueryHandler(
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IBlobService blobService)
        {
            this.unitOfWork = unitOfWork;
            this.mapper = mapper;
            this.blobService = blobService;
        }

        public async Task<IEnumerable<EventResponse>> Handle(GetEventsByQueryQuery request, CancellationToken cancellationToken)
        {
            Specification<EventEntity> specification = new IncludeMemberSpecification();

            if (!string.IsNullOrEmpty(request.Location))
            {
                var locationSpecification = new LocationSpecification(request.Location);

                specification = new AndSpecification<EventEntity>(locationSpecification, specification);
            }

            if (!string.IsNullOrEmpty(request.Category))
            {
                var categorySpecification = new CategorySpecification(request.Category);

                specification = new AndSpecification<EventEntity>(categorySpecification, specification);
            }

            if (request.EventTime.HasValue)
            {
                var dateSpecification = new DateSpecification(request.EventTime.Value);

                specification = new AndSpecification<EventEntity>(dateSpecification, specification);
            }

            var events = unitOfWork.EventRepository
            .GetEvents(specification)
                .ToList();

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

            return eventsResponse;
        }
    }
}
