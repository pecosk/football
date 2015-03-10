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
            Sets = new List<TournamentSet>();            
        }

        public TournamentMatch(TournamentTeam team1, TournamentTeam team2, int matchNumber)
        {
            Team1 = team1;
            Team2 = team2;
            MatchNumber = matchNumber;
            Sets = new List<TournamentSet>();            
        }

        public int Id { get; set; }

        public int MatchNumber { get; set; }

        public TournamentRound Round { get; set; }

        public TournamentTeam Team1 { get; set; }
        public TournamentTeam Team2 { get; set; }
        public List<TournamentSet> Sets { get; set; }

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

        internal static TournamentMatch CreateEmptyMatch(int matchNumber)
        {
            var match = new TournamentMatch
            {
                MatchNumber = matchNumber
            };
            match.Sets.Add(new TournamentSet());
            match.Sets.Add(new TournamentSet());
            match.Sets.Add(new TournamentSet());

            return match;
        }

        internal TournamentTeam GetWinner()
        {
            var numberOfWins = Sets.Count(x => x.Team1Score > x.Team2Score);
            return numberOfWins >= 2 ? Team1 : Team2;
        }
    }
}