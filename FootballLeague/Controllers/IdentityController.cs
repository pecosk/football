using FootballLeague.Filters;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System.Linq;
using System.Web.Http;

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
        public User Get()
        {
			var name = User.Identity.Name.Split('\\').Last();
            var user = _repository.GetUser(name);
            if(user == null || user.Inactive)
                return null;

            return user;
        }
    }
}