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
        private ITeamRepository _teamRepository;

        public MatchesController() : this(null, null, null)
        {
        }

        public MatchesController(IMatchesRepository matchRepository, IUsersRepository userRepository, ITeamRepository teamRepository)
        {
            _matchRepository = matchRepository ?? new MatchesRepository();
            _userRepository = userRepository ?? new UsersRepository();
            _teamRepository = teamRepository ?? new TeamRepository();
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

        public void Put(int teamId)
        {
            var user = GetCurrentUser();
            var team = _teamRepository.GetTeam(teamId);
            if (team == null)
                return;

            if (team.Contains(user))
                _teamRepository.RemoveMatchParticipantFromTeam(user, team);
            else
                _teamRepository.AddMatchParticipantToTeam(user, team);
        }

        private User GetCurrentUser()
        {
            var userName = User.Identity.Name.Split('\\').Last();
            return _userRepository.GetUser(userName);
        }
    }
}