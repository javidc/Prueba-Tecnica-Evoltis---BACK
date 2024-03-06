using Evoltis.Helpers;
using Evoltis.Models;
using Evoltis.Repositories.Interfaces;
using Evoltis.Services.Interfaces;

namespace Evoltis.Services
{
    public class TournamentService: ITournamentService
    {
        private readonly ITournamentRepository iTournamentRepository;

        public TournamentService(ITournamentRepository iTournamentRepository)
        {
            this.iTournamentRepository = iTournamentRepository;
        }

        public async Task<ResponseObjectJsonDto> GetTournaments()
        {
            List<Tournament> lstTournament = await iTournamentRepository.GetTournaments();

            return new ResponseObjectJsonDto
            {
                Code = (int)CodeHTTP.OK,
                Message = "Consulta exitosa",
                Response = lstTournament
            };
        }
    }
}
