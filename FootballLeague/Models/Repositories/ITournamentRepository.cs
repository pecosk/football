namespace FootballLeague.Models.Repositories
{
    using System;
    using System.Collections.Generic;
    using FootballLeague.Models.Tournament;

    public interface ITournamentRepository
    {
        Tournament GetById(int id);
        
        IEnumerable<Tournament> GetAll();

        Tournament CreateTournament(User user, Tournament tournament);
        
        void Update(Tournament tournament, TournamentTeam team);
        
        void Delete(Tournament tournament, TournamentTeam team);

        void Save(Tournament tournament);        
    }
}
