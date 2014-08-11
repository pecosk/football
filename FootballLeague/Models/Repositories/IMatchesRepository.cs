using System.Collections.Generic;

namespace FootballLeague.Models.Repositories
{
    public interface IMatchesRepository
    {
        Match InsertMatch(User user, Match match);
        IList<Match> GetPlanned();
    }
}