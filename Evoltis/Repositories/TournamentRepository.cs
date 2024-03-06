using Evoltis.Entity;
using Evoltis.Models;
using Evoltis.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Evoltis.Repositories
{
    public class TournamentRepository: ITournamentRepository
    {
        private readonly ApplicationDbContext dbContext;

        public TournamentRepository(ApplicationDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<List<Tournament>> GetTournaments()
        {
            return await dbContext.Tournaments.ToListAsync();
        }
    }
}
