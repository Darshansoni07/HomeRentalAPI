using AutoMapper;
using HomeRent.Dto;
using HomeRent.Models;

namespace HomeRent.Helper
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles() 
        {
            CreateMap<City,CityDto>().ReverseMap();
            CreateMap<City, CityUpdateDto>().ReverseMap();
            CreateMap<Properties,PropertyDto>().ReverseMap();
            CreateMap<HomeBook, BookingCreateDto>().ReverseMap();
            CreateMap<Users, UserAgentReqDto>().ReverseMap();
            CreateMap<Users, updateProfileDto>().ReverseMap();

        }

    }
}
