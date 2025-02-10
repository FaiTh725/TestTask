using Application.Shared.Responses;
using Event.Application.Models.Events;

namespace Event.Application.Interfaces
{
    public interface IEventService
    {
        Task<DataResponse<IEnumerable<EventResponse>>> GetEvents();

        Task<DataResponse<IEnumerable<EventResponse>>> GetEvents(int page, int size);

        Task<DataResponse<EventResponse>> GetEvent(long eventId);

        Task<DataResponse<EventResponse>> GetEvent(string eventName);

        Task<DataResponse<EventResponse>> RegistrNewEvent(EventRequest request);

        Task<BaseResponse> CancelEvent(long eventId);

        Task<BaseResponse> UpdateEvent(UpdateEventRequest request);

        Task<DataResponse<IEnumerable<EventResponse>>> GetEvents(string? location, string? category, DateTime? eventTime);
        
        // TODO: add images to events
    }
}
