using System.ComponentModel.DataAnnotations;

namespace Evoltis.Models.Dtos.ClubDtos
{
    public class ClubCreateDto
    {
        [Required(ErrorMessage = "El Nombre es requerido.")]
        public string Name { get; set; }
        [Required(ErrorMessage = "El CUIT es requerido.")]
        public string CUIT { get; set; }
        [Required(ErrorMessage = "La dirección es requerida.")]
        public string Address { get; set; }
        public IFormFile? ImageLogo { get; set; }
        [Required(ErrorMessage = "El nombre del estadio es requerido.")]
        public string StadiumName { get; set; }
        [Required(ErrorMessage = "El campeonato es requerido.")]
        public int IdTournament { get; set; }
    }
}
