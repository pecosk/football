using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FootballLeague.Models
{
    public class Tournament
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string Name { get; set; }

        public DateTime StartDate { get; set; }

        public List<Team> Participants { get; set; }

        public List<Match> Matches { get; set; } 
    }
}