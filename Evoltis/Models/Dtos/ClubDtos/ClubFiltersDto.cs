using Evoltis.Models.Dtos.PaginationDtos;

namespace Evoltis.Models.Dtos.ClubDtos
{
    public class ClubFiltersDto
    {
        public string? Name { get; set; }
        public int? IdTournament { get; set; }
        public PaginationDto Pagination { get; set; }
    }
}
