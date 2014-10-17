using System.Linq;
using System.Web;
using FootballLeague.Models;
using System.DirectoryServices;
using System;
using System.Configuration;

namespace FootballLeague.Services
{
    public class UsersADSearcher : IUsersADSearcher
    {
        public User LoadUserDetails(string userName)
        {
			var hostName = ConfigurationManager.AppSettings.Get ("adHostName");
			var adUserName = ConfigurationManager.AppSettings.Get("adUserName");
			var password = ConfigurationManager.AppSettings.Get ("adPassword");
			var entry = new DirectoryEntry();
			if (!string.IsNullOrEmpty (hostName)) 
			{
				entry.Path = string.Format ("Ldap://{0}:389", hostName);
			}
			if (!string.IsNullOrEmpty (adUserName) && !string.IsNullOrEmpty (password)) 
			{
				entry.Username = adUserName;
				entry.Password = password;
			}
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
			catch(Exception e)
            {
				Console.WriteLine (e.Message + Environment.NewLine + e.StackTrace);
				return new User { Name = userName, Mail = "local@user.sk", FirstName = "Local", LastName = "User" };
            }
        }
    }
}