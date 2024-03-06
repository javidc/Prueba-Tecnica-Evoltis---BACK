using Evoltis.Helpers;
using Evoltis.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Evoltis.Controllers
{
    [ApiController]
    [Route("api/tournament")]
    public class TournamentController : ControllerBase
    {
        private readonly ITournamentService iTournamentService;

        public TournamentController(ITournamentService iTournamentService)
        {
            this.iTournamentService = iTournamentService;
        }

        [HttpGet]
        public async Task<ActionResult> GetTournaments()
        {
            ResponseObjectJsonDto response = await iTournamentService.GetTournaments();

            if (response.Code != (int)CodeHTTP.OK)
            {
                return StatusCode(response.Code, response.Message);
            }

            return Ok(response);
        }
    }
}
