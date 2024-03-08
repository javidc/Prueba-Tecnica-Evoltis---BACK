﻿namespace Evoltis.Models.Dtos.ClubDtos
{
    public class ClubUpdateDto
    {
        public int IdClub { get; set; }
        public string Name { get; set; }
        public string CUIT { get; set; }
        public string Address { get; set; }
        public string? FileName { get; set; }
        public string StadiumName { get; set; }
        public int IdTournament { get; set; }
        public bool Active { get; set; }
    }
}
