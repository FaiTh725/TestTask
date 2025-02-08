using Application.Shared.Responses;
using Event.Application.Models.Events;
using Event.Application.Models.Members;

namespace Event.Application.Interfaces
{
    public interface IEventService
    {
        Task<DataResponse<IEnumerable<EventResponse>>> GetEvents();

        Task<DataResponse<EventResponse>> GetEvent(long eventId);

        Task<DataResponse<EventResponse>> GetEvent(string eventName);

        Task<DataResponse<EventResponse>> RegistrNewEvent(EventRequest request);

        Task<BaseResponse> CancelEvent(long eventId);
        Task<BaseResponse> UpdateEvent(UpdateEventRequest request);

        // TODO: get events by query
        
        // TODO: add images to events
    }
}
