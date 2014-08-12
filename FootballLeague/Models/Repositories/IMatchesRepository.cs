using System.Collections.Generic;

namespace FootballLeague.Models.Repositories
{
    public interface IMatchesRepository
    {
        Match InsertMatch(User user, Match match);
        IList<Match> GetPlanned();
        Match GetMatch(int id);
        void AddMatchParticipant(User user, Match match);
        void RemoveMatchParticipant(User user, Match match);
    }
}