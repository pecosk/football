using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using FootballLeague.Models.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace FootballLeague.Controllers
{
    public class TournamentTeamController : ApiController
    {
        private ITournamentRepository _tournamentRepository;
        private ITournamentTeamRepository _tournamentTeamRepository;
        private IUsersRepository _userRepository;        

        public TournamentTeamController(ITournamentRepository tournamentRepository, IUsersRepository userRepository, ITournamentTeamRepository tournamentTeamRepository)
        {
            _tournamentRepository = tournamentRepository;
            _tournamentTeamRepository = tournamentTeamRepository;
            _userRepository = userRepository;            
        }

        public void Get(int id)
        {
            _tournamentTeamRepository.GetById(id);
        }

        //
        // Post: /Team/
        public void Post(int id, [FromBody]TournamentTeam teamData)
        {
            var tournamnetId = teamData.TournamentId;
            var tournament = _tournamentRepository.GetById(id);
            var team = new TournamentTeam(teamData.TeamName);
            team.Member1 = GetCurrentUser();
            team.Member2 = _userRepository.GetUser(teamData.Member2.Id);
            _tournamentRepository.Update(tournament, team);
        }

        public void Delete(int id)
        {
            var team = _tournamentTeamRepository.GetById(id);
            var tournament = _tournamentRepository.GetById(team.Tournament.Id);                                    
            _tournamentRepository.Delete(tournament, team);
        }

        private User GetCurrentUser()
        {
            var userName = User.Identity.Name.Split('\\').Last();
            return _userRepository.GetUser(userName);
        }
    }
}
