using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FootballLeague.Controllers
{
    public class UsersController : ApiController
    {
        private UsersRepository _repository;

        public UsersController()
        {
            _repository = new UsersRepository();
        }

        // GET api/users
        public IEnumerable<User> Get()
        {
            //return new User[] {
            //    new User { Id = 1, Name = "Janko" },
            //    new User { Id = 2, Name = "Hrasko" }
            //};
            return _repository.GetAllUsers();
        }

        // GET api/users/5
        public string Get(int id)
        {
            return _repository.GetUser(id).Name;
        }

        // POST api/users
        public User Post()
        {
            var name = User.Identity.Name.Split('\\').Last();
            _repository.InsertUser(new User { Name = name });
            return _repository.GetUser(name);
        }

        // PUT api/users/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/users/5
        public void Delete(int id)
        {
            var user = _repository.GetUser(id);
            if (user == null || !User.Identity.Name.EndsWith('\\' + user.Name))
                return;

            _repository.DeleteUser(user.Id);
        }
    }
}