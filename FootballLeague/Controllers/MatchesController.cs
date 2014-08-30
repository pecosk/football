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
            var context = new FootballContext();
            _matchRepository = matchRepository ?? new MatchesRepository(context);
            _userRepository = userRepository ?? new UsersRepository(context);            
        }

        //Create new Match
        public void Post(Match match)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new UnauthorizedAccessException();

            if (match.Invites != null && match.Invites.Count > 0 && !_userRepository.UsersExist(match.Invites))
                return;

            _matchRepository.InsertMatch(user, new Match { PlannedTime = match.PlannedTime, Invites = match.Invites });
        }

        public IEnumerable<Match> Get()
        {
            return _matchRepository.GetPlanned();
        }

        public void Put(int id, [FromUri]int teamId)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new UnauthorizedAccessException();

            var match = _matchRepository.GetMatch(id);
            if (match == null)
                return;

            if (!_matchRepository.MatchContainsTeam(match, teamId))
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