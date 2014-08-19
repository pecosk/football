using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace FootballLeague.Controllers
{
    public class MatchesController : ApiController
    {
        private IMatchesRepository _matchRepository;
        private IUsersRepository _userRepository;        

        public MatchesController() : this(null, null)
        {
        }

        public MatchesController(IMatchesRepository matchRepository, IUsersRepository userRepository)
        {
            _matchRepository = matchRepository ?? new MatchesRepository();
            _userRepository = userRepository ?? new UsersRepository();            
        }

        public void Post(Match match)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new UnauthorizedAccessException();

            _matchRepository.InsertMatch(user, new Match { PlannedTime = match.PlannedTime });
        }

        public IEnumerable<Match> Get()
        {
            return _matchRepository.GetPlanned();
        }

        public void Put(int matchId, int teamId)
        {
            var user = GetCurrentUser();
            var match = _matchRepository.GetMatch(matchId);
            if (match == null)
                return;

            if (match.Contains(user))
                _matchRepository.RemoveMatchParticipantFromTeam(user, match, teamId);
            else
                _matchRepository.AddMatchParticipantToTeam(user, match, teamId);
        }

        private User GetCurrentUser()
        {
            var userName = User.Identity.Name.Split('\\').Last();
            return _userRepository.GetUser(userName);
        }
    }
}