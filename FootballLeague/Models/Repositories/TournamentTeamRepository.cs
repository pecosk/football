using FootballLeague.Models.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;

namespace FootballLeague.Models.Repositories
{
    public interface ITournamentTeamRepository
    {
        TournamentTeam GetById(int id);
    }

    public class TournamentTeamRepository: ITournamentTeamRepository
    {
        private readonly FootballContext _context;

        public TournamentTeamRepository(FootballContext context)
        {
            _context = context;
        }

        public TournamentTeam GetById(int id)
        {
            return _context.TournamentTeams.Include(x => x.Tournament).FirstOrDefault(x => x.Id == id);      
        }
    }
}