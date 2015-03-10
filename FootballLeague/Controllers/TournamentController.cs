namespace FootballLeague.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Http;

    using FootballLeague.Models;
    using FootballLeague.Models.Tournament;
    using FootballLeague.Models.Repositories;
    using FootballLeague.Services;    

    public class TournamentController : ApiController
    {
        private ITournamentRepository _tournamentRepository;
        private IUsersRepository _userRepository;        

        public TournamentController(ITournamentRepository tournamentRepository, IUsersRepository userRepository)
        {
            _tournamentRepository = tournamentRepository;
            _userRepository = userRepository;            
        }

        //Create new Match
        public void Post(Tournament tournament)
        {
            var user = GetCurrentUser();
            if (user == null)
                throw new UnauthorizedAccessException();

            _tournamentRepository.CreateTournament(user, tournament);            
        }

        public void Put(int id, [FromBody]TournamentTeam teamData)
        {
            var tournament = _tournamentRepository.GetById(id);
            var team = new TournamentTeam(teamData.TeamName);
            team.Member1 = GetCurrentUser();
            team.Member1 = _userRepository.GetUser(teamData.Member2.Id);
            _tournamentRepository.Update(tournament, team);
        }

        public void Put(int id, [FromUri]string state)
        {
            var tournament = _tournamentRepository.GetById(id);
            tournament.State = (TournamentState)Enum.Parse(typeof(TournamentState), state);
            if (tournament.State == TournamentState.InProgress)
            {
                CreateMatches(tournament);
            }
            _tournamentRepository.Save(tournament);
        }

        private void CreateMatches(Tournament tournament)
        {
            var pairs = tournament.Teams.Select((value, index) => new { value, index } ).GroupBy(x => x.index / 2, x => x.value);
            tournament.Matches = pairs.Select(
                x => {
                    var t = new TournamentMatch(x.First(), x.Last());
                    t.Sets.Add(new TournamentSet());
                    t.Sets.Add(new TournamentSet());
                    t.Sets.Add(new TournamentSet());
                    return t;
            }).ToList();
        }

        public IEnumerable<Tournament> Get()
        {
            return _tournamentRepository.GetAll();
        }

        public Tournament Get(int id)
        {
           return _tournamentRepository.GetById(id);
        }

        private User GetCurrentUser()
        {
            var userName = User.Identity.Name.Split('\\').Last();
            return _userRepository.GetUser(userName);
        }        
    }
}
