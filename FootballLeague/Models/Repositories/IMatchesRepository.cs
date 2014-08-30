﻿using System.Collections.Generic;

namespace FootballLeague.Models.Repositories
{
    public interface IMatchesRepository
    {
        Match InsertMatch(User user, Match match);
        IList<Match> GetPlanned();
        Match GetMatch(int id);
        void AddMatchParticipantToTeam(User user, Match match, int teamId);
        void RemoveMatchParticipantFromTeam(User user, Match match, int teamId);
        bool MatchContainsTeam(Match match, int teamId);
    }
}