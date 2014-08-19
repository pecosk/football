using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FootballLeague.Models.Repositories
{
    public class MatchesRepository : IMatchesRepository
    {
        private FootballContext _context;

        public MatchesRepository(FootballContext context = null)
        {
            _context = context ?? new FootballContext();
        }

        public Match InsertMatch(User user, Match match)
        {
            match.Creator = user;
            _context.Users.Attach(user);                        
            _context.Matches.Add(match);
            _context.SaveChanges();
            return match;
        }

        public IList<Match> GetPlanned()
        {
            return _context.Matches
                .Include(m => m.Team1)
                .Include(m => m.Team2)
                .Where(m => m.PlannedTime >= DateTime.Now)
                .OrderBy(m => m.PlannedTime)
                .ToList();
        }

        public Match GetMatch(int id)
        {
            return _context.Matches.FirstOrDefault(m => m.Id == id);
        }        
    }
}