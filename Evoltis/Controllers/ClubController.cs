using Evoltis.Helpers;
using Evoltis.Models.Dtos.ClubDtos;
using Evoltis.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Evoltis.Controllers
{
    [ApiController]
    [Route("api/club")]
    public class ClubController : ControllerBase
    {
        private readonly IClubService iClubService;
        private readonly ITournamentService iTournamentService;

        public ClubController(IClubService iClubService, ITournamentService iTournamentService)
        {
            this.iClubService = iClubService;
            this.iTournamentService = iTournamentService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateClub([FromForm] ClubCreateDto clubDto)
        {

            ResponseObjectJsonDto response = await iClubService.CreateClub(clubDto);
            if (response.Code != (int)CodeHTTP.OK)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("filters")]
        public async Task<ActionResult<ResponseObjectJsonDto>> GetClubsByFilters([FromQuery] ClubFiltersDto filters)
        {

            ResponseObjectJsonDto response = await iClubService.GetClubsByFilters(filters);

            if (response.Code != (int)CodeHTTP.OK)
            {
                return StatusCode(response.Code, response.Message);
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateClub([FromForm] ClubPatchDto clubDto)
        {

            ResponseObjectJsonDto response = await iClubService.UpdateClub(clubDto);
            if (response.Code != (int)CodeHTTP.OK)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
    }
}
