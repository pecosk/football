using FootballLeague.Models;

namespace FootballLeague.Services
{
    public interface IUsersADSearcher
    {
        User LoadUserDetails(string userName);
    }
}
