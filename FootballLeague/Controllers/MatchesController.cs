using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FootballLeague.Controllers
{
    public class MatchesController : ApiController
    {
        private IMatchesRepository _matchRepository;
        private IUsersRepository _userRepository;

        public MatchesController(IMatchesRepository matchRepository, IUsersRepository userRepository)
        {
            _matchRepository = matchRepository;
            _userRepository = userRepository;
        }

        //Create new Match
        public void Post(Match match)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new UnauthorizedAccessException();

            if (match.Invites != null && match.Invites.Count > 0 && !_userRepository.UsersExist(match.Invites))
                return;

            if (!_matchRepository.IsTimeSlotFree(match.PlannedTime))
                throw new ArgumentException("Time slot for match already taken, choose another time.");

            if (match.PlannedTime<DateTime.Now)
                throw new ArgumentException("Cannot create a match in the past.");

            _matchRepository.InsertMatch(user, new Match { PlannedTime = match.PlannedTime, Invites = match.Invites });
        }

        public IEnumerable<Match> Get()
        {
            return _matchRepository.GetAll();
        }

        public void Put(int id, [FromUri]int teamId)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new UnauthorizedAccessException();

            var match = _matchRepository.GetMatch(id);
            if (match == null)
                return;

            if (match.PlannedTime < DateTime.Now)
                throw new ArgumentException("Match already started.");

            if (!_matchRepository.MatchContainsTeam(match, teamId))
                return;

            if (match.Contains(user))
                _matchRepository.RemoveMatchParticipantFromTeam(user, match, teamId);
            else
                _matchRepository.AddMatchParticipantToTeam(user, match, teamId);
        }

        public void Put(int id, [FromUri] int t1Score, [FromUri] int t2Score)
        {
            var match = _matchRepository.GetMatch(id);
            if (match == null)
                return;

            _matchRepository.UpdateScore(match, t1Score, t2Score);
        }

        private User GetCurrentUser()
        {
            var userName = User.Identity.Name.Split('\\').Last();
            return _userRepository.GetUser(userName);
        }
    }
}