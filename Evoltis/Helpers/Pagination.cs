using Evoltis.Models.Dtos.PaginationDtos;

namespace Evoltis.Helpers
{
    public static class Pagination
    {
        public static IList<T> Page<T>(this IList<T> queryable, PaginationDto paginationDTO)
        {
            return queryable
                .Skip((paginationDTO.Page - 1) * paginationDTO.AmountRegistersPage)
                .Take(paginationDTO.AmountRegistersPage).ToList();
        }
    }
}
