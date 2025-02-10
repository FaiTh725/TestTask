using AutoMapper;
using Event.Application.Models.Members;
using Event.Domain.Entities;

namespace Event.Application.Mappings
{
    public class EventMemberProfile : Profile
    {
        public EventMemberProfile()
        {
            CreateMap<EventMember, MemberResponse>()
                .ForMember(dest =>
                    dest.FirstName,
                    opt => opt.MapFrom(str => str.FirstName))
                .ForMember(dest =>
                    dest.SecondName,
                    opt => opt.MapFrom(str => str.SecondName))
                .ForMember(dest =>
                    dest.Email,
                    opt => opt.MapFrom(str => str.Email))
                .ForMember(dest =>
                    dest.BirthDate,
                    opt => opt.MapFrom(str => str.BirthDate))
                .ForMember(dest =>
                    dest.Id,
                    opt => opt.MapFrom(str => str.Id))
                .ForMember(dest =>
                    dest.RegistrationDate,
                    opt => opt.MapFrom(str => str.RegistrationDate));
        }
    }
}
