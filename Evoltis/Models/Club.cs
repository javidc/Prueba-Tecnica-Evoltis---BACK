using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Evoltis.Models
{
    public class Club
    {
        [Key]
        public int IdClub { get; set; }
        public string Name { get; set; }
        public string CUIT { get; set; }
        public string Address { get; set; }
        public string? FileName { get; set; }
        public string StadiumName { get; set; }
        public int IdTournament { get; set; }
        [ForeignKey("IdTournament")]
        public Tournament Tournament { get; set; }
        public bool Active { get; set; }
    }
}
