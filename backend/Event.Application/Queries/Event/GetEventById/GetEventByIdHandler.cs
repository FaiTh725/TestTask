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
        private readonly IMapper mapper;

        public GetEventByIdHandler(
            IUnitOfWork unitOfWork, 
            IBlobService blobService,
            IMapper mapper)
        {
            this.unitOfWork = unitOfWork;
            this.blobService = blobService;
            this.mapper = mapper;
        }

        public async Task<EventResponse> Handle(GetEventByIdQuery request, CancellationToken cancellationToken)
        {
            var eventEntity = await unitOfWork.EventRepository
                .GetEventWithMembers(request.Id, cancellationToken);

            if (eventEntity is null)
            {
                throw new NotFoundApiException("Event Does Not Exist");
            }

            var eventResponse = mapper.Map<EventResponse>(eventEntity);
            eventResponse.Members = mapper
                .Map<IEnumerable<MemberResponse>>(eventEntity.Members);
            eventResponse.UrlImages = await blobService
                .DownloadBlobs(eventEntity.ImagesFolder, cancellationToken);

            return eventResponse;
        }
    }
}
