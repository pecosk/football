using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;  
using System.Web;

namespace FootballLeague.Models.Repositories
{
    public class TournamentMatchRepository : ITournamentMatchRepository
    {
        private FootballContext _context;

        public TournamentMatchRepository(FootballContext context)
        {
            _context = context;
        }

        public Tournament.TournamentMatch GetById(int id)
        {
            return _context.TournamentMatches
                .Include(x => x.Round)
                .Include(x => x.Round.Tournament)
                .Include(x => x.Team1)
                .Include(x => x.Team2)
                .Include(x => x.Round.Tournament.Rounds)
                .Include(x => x.Round.Tournament.Rounds.Select(y => y.Matches))
                .FirstOrDefault(x => x.Id == id);
        }


        public void Save(Tournament.TournamentMatch tournamentMatch)
        {
            _context.Entry(tournamentMatch).State = EntityState.Modified;
            _context.SaveChanges();
        }
    }
}