using Evoltis.Entity;
using Evoltis.Models;
using Evoltis.Models.Dtos.ClubDtos;
using Evoltis.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evoltis.Repositories
{
    public class ClubRepository: IClubRepository
    {
        private readonly ApplicationDbContext dbContext;
        public ClubRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<bool> ClubExistName(string name)
        {
            return await dbContext.Clubs.AnyAsync(c => c.Name.ToUpper().Equals(name.ToUpper()));
        }

        public async Task<bool> ClubExistCuit(string cuit)
        {
            return await dbContext.Clubs.AnyAsync(c => c.CUIT.Equals(cuit));
        }

        public async Task<bool> ClubHaveImage(int idClub)
        {
            return await dbContext.Clubs.AnyAsync(u => u.IdClub == idClub && u.FileName != null);
        }

        public async Task<bool> ClubUpdateExistsName(int idClub, string name)
        {
            return await dbContext.Clubs.AnyAsync(u => u.Name.ToLower() == name.ToLower() && u.IdClub != idClub);
        }

        public async Task CreateClub(Club club)
        {
            await dbContext.Clubs.AddAsync(club);
        }

        public async Task<Club> GetClub(int idClub)
        {
            return await dbContext.Clubs.AsNoTracking().Where(c => c.IdClub == idClub).FirstOrDefaultAsync();
        }

        public async Task<List<Club>> GetClubsByFilters(ClubFiltersDto filters)
        {
            List<Club> listClubs = await dbContext.Clubs
                    .Include(c => c.Tournament)
                    .OrderBy(c => c.Name)
                    .ToListAsync();

            if (filters.Name != null && filters.Name != "")
            {
            listClubs = listClubs.Where(c => c.Name.ToLower().Contains(filters.Name)).ToList();
            }
            if (filters.IdTournament != null && filters.IdTournament != 0)
            {
            listClubs = listClubs.Where(c => c.IdTournament.Equals(filters.IdTournament)).ToList();
            }

            return listClubs;
        }

        public void UpdateClub(Club club)
        {
            dbContext.Update(club);
        }
    }
}
