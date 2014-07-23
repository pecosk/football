using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System;
using System.Linq;
using System.Web.Http;

namespace FootballLeague.Controllers
{
    public class IdentityController : ApiController
    {
        private UsersRepository _repository;

        public IdentityController()
        {
            _repository = new UsersRepository();
        }

        public User Get()
        {
            var name = User.Identity.Name.Split('\\').Last();
            var user = _repository.GetUser(name);
            if (user == null)
                throw new NullReferenceException();

            return user;
        }
    }
}