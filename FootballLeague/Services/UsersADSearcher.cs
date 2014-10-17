using System.Linq;
using System.Web;
using FootballLeague.Models;
using System.DirectoryServices;

namespace FootballLeague.Services
{
    public class UsersADSearcher : IUsersADSearcher
    {
        public User LoadUserDetails(string userName)
        {
            var entry = new DirectoryEntry();
            var searcher = new DirectorySearcher(entry);
            searcher.Filter = "(&(objectClass=user)(sAMAccountName=" + userName + "))";
            searcher.PropertiesToLoad.Add("givenName");
            searcher.PropertiesToLoad.Add("sn");
            searcher.PropertiesToLoad.Add("mail");
            try
            {
                var userProps = searcher.FindOne().Properties;
                var mail = userProps["mail"][0].ToString();
                var first = userProps["givenName"][0].ToString();
                var last = userProps["sn"][0].ToString();

                return new User { Name = userName, Mail = mail, FirstName = first, LastName = last };
            }
            catch
            {
				return new User { Name = userName, Mail = "local@user.sk", FirstName = "Local", LastName = "User" };
            }
        }
    }
}