namespace FootballLeague.Models.Repositories
{
    public interface IMatchesRepository
    {
        Match InsertMatch(User user, Match match);
    }
}