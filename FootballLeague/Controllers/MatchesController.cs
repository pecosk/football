using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using FootballLeague.Services;
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
        private INotifier _notifier;

        public MatchesController(IMatchesRepository matchRepository, IUsersRepository userRepository, INotifier notifier)
        {
            _matchRepository = matchRepository;
            _userRepository = userRepository;
            _notifier = notifier;
        }

        //Create new Match
        public void Post(Match match)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new UnauthorizedAccessException();

            List<User> verifiedInvites = null;
            if (match.Invites != null && match.Invites.Count > 0)
            {
                var dbInvites = _userRepository.GetVerifiedUsers(match.Invites);
                if (dbInvites == null && dbInvites.Count() == 0)
                    return;

                verifiedInvites = dbInvites.ToList();
            }
            if (!_matchRepository.IsTimeSlotFree(match.PlannedTime))
                throw new ArgumentException("Time slot for match already taken, choose another time.");

            _matchRepository.InsertMatch(user, new Match { PlannedTime = match.PlannedTime, Invites = verifiedInvites });
            _notifier.Notify(user, verifiedInvites, match.PlannedTime);
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