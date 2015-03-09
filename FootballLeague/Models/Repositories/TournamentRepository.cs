namespace FootballLeague.Models.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;      
    using System.Data.Entity;    
    using Tournament = FootballLeague.Models.Tournament.Tournament;
    using FootballLeague.Models.Tournament;

    public class TournamentRepository : ITournamentRepository
    {
        private FootballContext _context;

        public TournamentRepository(FootballContext context)
        {
            _context = context;
        }        

        public Tournament GetById(int id)
        {
            return _context.Tournaments
                .Include(t => t.Creator)
                .Include(t => t.Teams)
                .Include(t => t.Teams.Select(x => x.Member1))
                .Include(t => t.Teams.Select(x => x.Member2))
                .Include(t => t.Matches)
                .FirstOrDefault(m => m.Id == id);
        }

        public IEnumerable<Tournament> GetAll()
        {
            return _context.Tournaments
                .Include(t => t.Creator)
                .Include(t => t.Teams)
                .Include(t => t.Teams.Select(x => x.Member1))
                .Include(t => t.Teams.Select(x => x.Member2))
                .Include(t => t.Matches)
                .ToList();
        }

        public Tournament CreateTournament(User user, Tournament tournament)
        {
            tournament.Creator = user;
            _context.Users.Attach(user);
            _context.Tournaments.Add(tournament);
            _context.SaveChanges();
            
            return tournament;
        }

        public void Update(Tournament tournament, TournamentTeam team)
        {
            tournament.Teams.Add(team);
            _context.TournamentTeams.Add(team);
            _context.Users.Attach(team.Member1);
            _context.Users.Attach(team.Member2);
            _context.Tournaments.Attach(tournament);

            _context.SaveChanges();
        }


        public void Delete(Tournament tournament, int teamId)
        {            
            var teamToDelete =_context.TournamentTeams.FirstOrDefault(x => x.Id == teamId);
            tournament.Teams.Remove(teamToDelete);
            _context.TournamentTeams.Remove(teamToDelete);

            _context.Tournaments.Attach(tournament);
            _context.SaveChanges();
        }


        public void Delete(Tournament tournament, TournamentTeam team)
        {
            tournament.Teams.Remove(team);
            _context.TournamentTeams.Remove(team);

            _context.Tournaments.Attach(tournament);
            _context.SaveChanges();
        }


        public void Save(Tournament tournament)
        {
            tournament.Matches.ForEach(x => _context.TournamentMatches.Add(x));            
            tournament.Matches.ForEach(x => x.Sets.ForEach(y => _context.TournamentSets.Add(y)));            
            _context.Entry(tournament).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}