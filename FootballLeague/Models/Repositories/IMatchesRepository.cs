using System;
using System.Collections.Generic;

namespace FootballLeague.Models.Repositories
{
    public interface IMatchesRepository
    {
        Match InsertMatch(User user, Match match);
        IList<Match> GetPlanned();
        IList<Match> GetAll();
        Match GetMatch(int id);
        void AddMatchParticipantToTeam(User user, Match match, int teamId);
        void RemoveMatchParticipantFromTeam(User user, Match match, int teamId);
        bool MatchContainsTeam(Match match, int teamId);
        bool IsTimeSlotFree(DateTime plannedTime);
        void UpdateScore(Match match, int t1Score, int t2Score);
    }
}