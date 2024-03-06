using Evoltis.Models;
using Evoltis.Models.Dtos.ClubDtos;

namespace Evoltis.Repositories.Interfaces
{
    public interface IClubRepository
    {
        Task<bool> ClubExistCuit(string cuit);
        Task<bool> ClubExistName(string name);
        Task<bool> ClubHaveImage(int idClub);
        Task<bool> ClubUpdateExistsName(int idClub, string name);
        Task CreateClub(Club club);
        Task<Club> GetClub(int idClub);
        Task<List<Club>> GetClubsByFilters(ClubFiltersDto filters);
        void UpdateClub(Club club);

    }
}
