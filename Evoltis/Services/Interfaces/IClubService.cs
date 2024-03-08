using Evoltis.Helpers;
using Evoltis.Models.Dtos.ClubDtos;

namespace Evoltis.Services.Interfaces
{
    public interface IClubService
    {
        Task<ResponseObjectJsonDto> CreateClub(ClubCreateDto clubDto);
        Task<ResponseObjectJsonDto> DisableClub(int idClub);
        Task<ResponseObjectJsonDto> GetClubsByFilters(ClubFiltersDto filters);
        Task<ResponseObjectJsonDto> GetClubById(int idClub);
        Task<FileStream> GetImageClubById(int idClub);
        Task<ResponseObjectJsonDto> UpdateClub(ClubPatchDto clubDto);
    }
}
