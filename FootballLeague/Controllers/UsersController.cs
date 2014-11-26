using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using FootballLeague.Services;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Diagnostics;
using System;

namespace FootballLeague.Controllers
{
    public class UsersController : ApiController
    {
        private IUsersRepository _repository;
        private IUsersADSearcher _adSearcher;

        public UsersController(IUsersRepository repository, IUsersADSearcher adSearcher)
        {
            _repository = repository;
            _adSearcher = adSearcher;
        }

        // GET api/users
		[NoCacheHeaderActionFilter]
        public IEnumerable<User> Get()
        {
			var sw = Stopwatch.StartNew ();
			var users = _repository.GetAllUsers ();
			sw.Stop ();
			Console.WriteLine ("users " + sw.ElapsedMilliseconds);
			return users;
        }

        // GET api/users/5
		[NoCacheHeaderActionFilter]
        public User Get(int id)
        {
            return _repository.GetUser(id);
        }

        // POST api/users
        public User Post()
        {
            var name = User.Identity.Name.Split('\\').Last();
            var user = _adSearcher.LoadUserDetails(name);
            _repository.InsertUser(user);
            return _repository.GetUser(name);
        }

        // PUT api/users/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/users/5
        public void Delete(int id)
        {
            var user = _repository.GetUser(id);
            var userName = User.Identity.Name.Split('\\').Last();
            if (user == null || userName != user.Name)
                return;

            _repository.DeleteUser(user.Id);
        }
    }
}