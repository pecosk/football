namespace FootballLeague.Models.Repositories
{
    public class MatchesRepository : IMatchesRepository
    {
        private FootballContext _context;

        public MatchesRepository()
        {
            _context = new FootballContext();
        }

        public Match InsertMatch(User user, Match match)
        {
            match.Creator = user;
            _context.Matches.Add(match);
            _context.SaveChanges();
            return match;
        }
    }
}