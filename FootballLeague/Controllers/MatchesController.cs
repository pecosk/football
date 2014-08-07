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

        public MatchesController(IMatchesRepository matchRepository, IUsersRepository userRepository)
        {
            _matchRepository = matchRepository;
            _userRepository = userRepository;
        }

        public void Post(Match match)
        {
            var userName = User.Identity.Name.Split('\\').Last();
            var user = _userRepository.GetUser(userName);
            if (user == null)
                throw new UnauthorizedAccessException();

            _matchRepository.InsertMatch(user, new Match { PlannedTime = match.PlannedTime });
        }
    }
}