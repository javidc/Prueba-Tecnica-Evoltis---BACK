using System.ComponentModel.DataAnnotations;

namespace Evoltis.Models.Dtos.ClubDtos
{
    public class ClubPatchDto
    {
        [Required(ErrorMessage = "El ID es requerido.")]
        public int IdClub { get; set; }
        [Required(ErrorMessage = "El nombre es requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El CUIT es requerido.")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "CUIT debe contener solo 11 caracteres.")]
        public string CUIT { get; set; }
        [Required(ErrorMessage = "La dirección es requerido.")]
        public string Address { get; set; }
        public IFormFile? ImageLogo { get; set; }
        [Required(ErrorMessage = "El nombre del estadio es requerido.")]
        public string StadiumName { get; set; }
        [Required(ErrorMessage = "El campeonato es requerido.")]
        public int IdTournament { get; set; }
        [Required(ErrorMessage = "El campo activo es requerido.")]
        public bool Active { get; set; }
    }
}
