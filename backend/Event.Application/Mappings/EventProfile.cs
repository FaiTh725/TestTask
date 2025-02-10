using AutoMapper;
using Event.Application.Models.Events;
using Event.Domain.Entities;

namespace Event.Application.Mappings
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventEntity, EventResponse>()
                .ForMember(dest =>
                    dest.UrlImages,
                    opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Members,
                    opt => opt.Ignore())
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(str => str.Id))
                .ForMember(dest =>
                    dest.Name,
                    opt => opt.MapFrom(str => str.Name))
                .ForMember(dest =>
                    dest.Description,
                    opt => opt.MapFrom(str => str.Description))
                .ForMember(dest =>
                    dest.Category,
                    opt => opt.MapFrom(str => str.Category))
                .ForMember(dest =>
                    dest.Location,
                    opt => opt.MapFrom(str => str.Location))
                .ForMember(dest =>
                    dest.MaxMembers,
                    opt => opt.MapFrom(str => str.MaxMember))
                .ForMember(dest =>
                    dest.TimeEvent,
                    opt => opt.MapFrom(str => str.TimeEvent));
        }
    }
}
