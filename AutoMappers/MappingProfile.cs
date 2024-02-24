using AutoMapper;
using WebApplication1.DTOs;
using WebApplication1.Models;

namespace WebApplication1.AutoMappers
{
    public class MappingProfile: Profile
    {
        public MappingProfile() 
        {
            CreateMap<BeerInsertDTO, Beer>();
            CreateMap<Beer, BeerDTO>()
                .ForMember(dto => dto.Id, m => m.MapFrom(b => b.BeerId));
            CreateMap<BeerUpdateDTO, Beer>();
        }
    }
}
