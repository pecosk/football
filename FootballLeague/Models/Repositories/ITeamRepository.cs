namespace FootballLeague.Models.Repositories
{
    public interface ITeamRepository
    {
        Team GetTeam(int id);

        void AddMatchParticipantToTeam(User user, Team match);

        void RemoveMatchParticipantFromTeam(User user, Team match);
    }
}