using Evoltis.Models;

namespace Evoltis.Repositories.Interfaces
{
    public interface ITournamentRepository
    {
        Task<List<Tournament>> GetTournaments();
    }
}
