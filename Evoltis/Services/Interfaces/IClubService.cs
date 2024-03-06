using Evoltis.Helpers;
using Evoltis.Models.Dtos.ClubDtos;

namespace Evoltis.Services.Interfaces
{
    public interface IClubService
    {
        Task<ResponseObjectJsonDto> CreateClub(ClubCreateDto clubDto);
        Task<ResponseObjectJsonDto> GetClubsByFilters(ClubFiltersDto filters);
        Task<ResponseObjectJsonDto> UpdateClub(ClubPatchDto clubDto);
    }
}
