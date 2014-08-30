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

        public Match GetMatch(int id)
        {
            return _context.Matches.Include(m => m.Creator)
                .Include(m => m.Team1)
                .Include(m => m.Team1.Member1)
                .Include(m => m.Team1.Member2)
                .Include(m => m.Team2)
                .Include(m => m.Team2.Member1)
                .Include(m => m.Team2.Member2)
                .FirstOrDefault(m => m.Id == id);
        }

        public Match InsertMatch(User user, Match match)
        {
            match.Creator = user;
            _context.Users.Attach(user);
            if (match.Invites != null)
            {
                match.Invites = match.Invites.Select(i => _context.Users.FirstOrDefault(u => u.Id == i.Id))
                                    .Where(u => u != null).ToList();
                match.Invites.ForEach(u => _context.Users.Attach(u));
            }
            _context.Teams.Add(match.Team1);
            _context.Teams.Add(match.Team2);             
            _context.Matches.Add(match);
            _context.SaveChanges();
            return match;
        }

        public IList<Match> GetPlanned()
        {
            return _context.Matches
                .Include(m => m.Creator)
                .Include(m => m.Team1)
                .Include(m => m.Team1.Member1)
                .Include(m => m.Team1.Member2)
                .Include(m => m.Team2)
                .Include(m => m.Team2.Member1)
                .Include(m => m.Team2.Member2)
                .Where(m => m.PlannedTime >= DateTime.Now)
                .OrderBy(m => m.PlannedTime)
                .ToList();
        }

        public void AddMatchParticipantToTeam(User user, Match match, int teamId)
        {
            var team = match.GetTeam(teamId);
            if (team == null || team.IsFull || match.Contains(user))
                return;

            if (team.SetMember(user))
            {                
                _context.Users.Attach(user);
                _context.SaveChanges();
            }                        
        }

        public void RemoveMatchParticipantFromTeam(User user, Match match, int teamId)
        {
            var team = match.GetTeam(teamId);
            if (team == null || team.IsEmpty || !team.Contains(user))
                return;

            if (team.RemoveMember(user))
            {
                _context.SaveChanges();
            }            
        }


        public bool MatchContainsTeam(Match match, int teamId)
        {
            var team = match.GetTeam(teamId);
            return team != null;
        }
    }
}