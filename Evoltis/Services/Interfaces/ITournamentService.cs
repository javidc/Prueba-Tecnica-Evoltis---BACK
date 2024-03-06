using Evoltis.Helpers;

namespace Evoltis.Services.Interfaces
{
    public interface ITournamentService
    {
        Task<ResponseObjectJsonDto> GetTournaments();
    }
}
