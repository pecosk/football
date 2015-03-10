using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;

namespace FootballLeague.Models.Tournament
{
    public class TournamentMatch
    {
        public TournamentMatch()
        {
            Team1 = new TournamentTeam();
            Team2 = new TournamentTeam();
            Sets = new List<TournamentSet>();            
        }

        public TournamentMatch(TournamentTeam team1, TournamentTeam team2)
        {
            Team1 = team1;
            Team2 = team2;            
        }

        public int Id { get; set; }
        public virtual Tournament Tournament { get; set; }
        public virtual TournamentTeam Team1 { get; set; }
        public virtual TournamentTeam Team2 { get; set; }
        public virtual List<TournamentSet> Sets { get; set; }

        public bool Contains(User user)
        {
            return Team1.Contains(user) || Team2.Contains(user);
        }

        public TournamentTeam GetTeam(int teamId)
        {
            if (Team1.Id == teamId) return Team1;
            if (Team2.Id == teamId) return Team2;

            return null;
        }        
    }
}