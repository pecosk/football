using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace FootballLeague.Controllers
{
    public class UsersController : ApiController
    {
        private IUsersRepository _repository;

        public UsersController() : this(null)
        {
        }

        public UsersController(IUsersRepository repository)
        {
            _repository = repository ?? new UsersRepository();
        }

        // GET api/users
        public IEnumerable<User> Get()
        {
            return _repository.GetAllUsers();
        }

        // GET api/users/5
        public User Get(int id)
        {
            return _repository.GetUser(id);
        }

        // POST api/users
        public User Post()
        {
            var name = User.Identity.Name.Split('\\').Last();
            _repository.InsertUser(new User { Name = name });
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