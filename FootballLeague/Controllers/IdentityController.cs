using FootballLeague.Filters;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System.Linq;
using System.Web.Http;
using System.Diagnostics;
using System;

namespace FootballLeague.Controllers
{
    public class IdentityController : ApiController
    {
        private IUsersRepository _repository;

        public IdentityController(IUsersRepository repository)
        {
            _repository = repository;
        }

        [NullObjectActionFilter]
		[NoCacheHeaderActionFilter]
        public User Get()
        {
			var sw = Stopwatch.StartNew ();
			var name = User.Identity.Name.Split('\\').Last();
            var user = _repository.GetUser(name);
			sw.Stop ();
			Console.WriteLine ("identity " + sw.ElapsedMilliseconds);
            if(user == null || user.Inactive)
                return null;

            return user;
        }
    }
}