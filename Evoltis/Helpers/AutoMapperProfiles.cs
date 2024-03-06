using AutoMapper;
using Evoltis.Models;
using Evoltis.Models.Dtos.ClubDtos;

namespace Evoltis.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<ClubCreateDto, Club>();
            CreateMap<Club, ClubGetDto>()
            .ForMember(
                dest => dest.Tournament,
                opt => opt.MapFrom(src => src.Tournament.Name)
            );
            CreateMap<ClubPatchDto, Club>();
        }
    }
}
