using FootballLeague.Models.Repositories;
using FootballLeague.Models.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace FootballLeague.Controllers
{
    public class TournamentMatchController : ApiController
    {
        private ITournamentMatchRepository _tournamentRepository;
        public TournamentMatchController(ITournamentMatchRepository tournamentRepsitory)
        {
            _tournamentRepository = tournamentRepsitory;
        }

        public void Put(int id, [FromBody]TournamentMatch match)
        {
            var tournamentMatch = _tournamentRepository.GetById(id);
            tournamentMatch.Sets = match.Sets;

            var tournamentSize = tournamentMatch.Round.Tournament.Size;
            var nextRoundNumber = tournamentMatch.Round.RoundNumber + 1;            
                       
            var nextRound = tournamentMatch.Round.Tournament.Rounds[nextRoundNumber];
            var numberOfMatches = tournamentSize / (int)Math.Pow(2, nextRoundNumber + 1);
            var matchIndex = tournamentMatch.MatchNumber / numberOfMatches;
            var teamIndex = tournamentMatch.MatchNumber % 2;
            if(teamIndex == 0)
            {
                nextRound.Matches[matchIndex].Team1 = tournamentMatch.GetWinner();
            }
            else
            {
                nextRound.Matches[matchIndex].Team2 = tournamentMatch.GetWinner();
            }

            _tournamentRepository.Save(tournamentMatch);
        }
    }
}
