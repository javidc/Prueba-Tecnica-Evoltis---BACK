using Evoltis.Helpers;
using Evoltis.Models.Dtos.ClubDtos;
using Evoltis.Services.Interfaces;
using FluentValidation;
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
        private readonly IValidator<ClubCreateDto> iClubCreateValidator;
        private readonly IValidator<ClubPatchDto> iClubPatchValidator;

        public ClubController(IClubService iClubService,
            IValidator<ClubCreateDto> iClubCreateValidator,
            IValidator<ClubPatchDto> iClubPatchValidator,
            ITournamentService iTournamentService)
        {
            this.iClubService = iClubService;
            this.iClubCreateValidator = iClubCreateValidator;
            this.iClubPatchValidator = iClubPatchValidator;
            this.iTournamentService = iTournamentService;
        }

        [HttpPost]
        public async Task<ActionResult> CreateClub([FromForm] ClubCreateDto clubDto)
        {
            var validationResult = await iClubCreateValidator.ValidateAsync(clubDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

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

        [HttpGet]
        [Route("{idClub:int}")]
        public async Task<ActionResult<ResponseObjectJsonDto>> GetClubById(int idClub)
        {

            ResponseObjectJsonDto response = await iClubService.GetClubById(idClub);

            if (response.Code != (int)CodeHTTP.OK)
            {
                return StatusCode(response.Code, response.Message);
            }

            return Ok(response);
        }

        [HttpGet]
        [Route("image/{idClub:int}")]
        public async Task<ActionResult<ResponseObjectJsonDto>> GetImageClubById(int idClub)
        {
            FileStream file = await iClubService.GetImageClubById( idClub);

            if (file == null)
            {
                return NotFound("No se pudo encontrar la imagen del club seleccionado");
            }

            return File(file, $"image/{Path.GetExtension(file.Name).Trim('.').ToLower()}");
        }

        [HttpPatch]
        [Route("disable/{idClub:int}")]
        public async Task<ActionResult> DisableClub(int idClub)
        {

            ResponseObjectJsonDto response = await iClubService.DisableClub(idClub);
            if (response.Code != (int)CodeHTTP.OK)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }

        [HttpPatch]
        public async Task<ActionResult> UpdateClub([FromForm] ClubPatchDto clubDto)
        {
            var validationResult = await iClubPatchValidator.ValidateAsync(clubDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            ResponseObjectJsonDto response = await iClubService.UpdateClub(clubDto);
            if (response.Code != (int)CodeHTTP.OK)
            {
                return StatusCode(response.Code, response);
            }

            return Ok(response);
        }
    }
}
