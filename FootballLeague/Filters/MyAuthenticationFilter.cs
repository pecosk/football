using System.Linq;
using System.Web.Http.Filters;
using System.Threading.Tasks;
using System.Security.Principal;
using System.Threading;
using System.Configuration;
using System.Text.RegularExpressions;

namespace FootballLeague.Filters
{
	public class MyAuthenticationFilter : ActionFilterAttribute, IAuthenticationFilter
	{

		public Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
		{
			if(context.Principal == null)
			{
				string userName;
				if (context.Request.Headers.Contains ("X-Forwarded-User")) 
				{
					userName = context.Request.Headers.GetValues("X-Forwarded-User").First();
				} 
				else 
				{
					userName = ConfigurationManager.AppSettings ["fakeUser"];
				}
				var suffixDomainRemover = new Regex ("@.*");
				userName = suffixDomainRemover.Replace (userName, "");
				var identity = new GenericIdentity(userName, "Basic");
				string[] roles = new string[]{ };
				var principal = new GenericPrincipal(identity, roles);

				context.Principal = principal;
			}

			return Task.FromResult(0);
		}

		public Task ChallengeAsync (HttpAuthenticationChallengeContext context, System.Threading.CancellationToken cancellationToken)
		{
			return Task.FromResult (0);
		}
	}
}