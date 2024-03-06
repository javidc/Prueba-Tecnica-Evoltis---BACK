using System.ComponentModel.DataAnnotations;

namespace Evoltis.Models
{
    public class Tournament
    {
        [Key]
        public int IdTournament { get; set; }
        public string Name { set; get; }

    }
}
