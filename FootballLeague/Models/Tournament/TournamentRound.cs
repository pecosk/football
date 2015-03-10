using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FootballLeague.Models.Tournament
{
    public class TournamentRound
    {
        public TournamentRound()
        {
            Matches = new List<TournamentMatch>();
        }

        public TournamentRound(int roundNumber, List<TournamentMatch> matches)
        {
            RoundNumber = roundNumber;
            Matches = matches;
        }

        public int Id { get; set; }

        public int RoundNumber { get; set; }

        public List<TournamentMatch> Matches { get; set; }        

        public Tournament Tournament { get; set; }

        public int Size
        {
            get { return Matches.Count; }
        }

        internal static TournamentRound CreateEmptyRound(int tournamentSize, int roundNumber)
        {
            var numberOfMatches = tournamentSize / (int)Math.Pow(2, roundNumber + 1);
            var matches = Enumerable.Range(0, numberOfMatches).Select(x => TournamentMatch.CreateEmptyMatch(x)).ToList();
            return new TournamentRound(roundNumber, matches);
        }
    }
}