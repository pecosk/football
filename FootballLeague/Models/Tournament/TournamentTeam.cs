using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace FootballLeague.Models.Tournament
{
    public class TournamentTeam
    {
        public TournamentTeam()
        {
        }
        
        public TournamentTeam(string teamName)
        {            
            TeamName = teamName;            
        }

        public TournamentTeam(Tournament tournament, string teamName, User member1, User member2)
        {            
            Tournament = tournament;
            TeamName = teamName;
            Member1 = member1;
            Member2 = member2;
        }
        
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string TeamName { get; set; }

        public User Member1 { get; set; }

        public User Member2 { get; set; }

        public bool Contains(User user)
        {
            return user.Equals(Member1) || user.Equals(Member2);
        }

        public int TournamentId { get; set; }
        public Tournament Tournament { get; set; }
    }
}