using FootballLeague.Models.Tournament;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FootballLeague.Models.Repositories
{
    public interface ITournamentMatchRepository
    {
        TournamentMatch GetById(int id);

        void Save(TournamentMatch tournamentMatch);
    }
}
