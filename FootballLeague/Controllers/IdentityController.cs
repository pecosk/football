﻿using FootballLeague.Filters;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System.Linq;
using System.Web.Http;

namespace FootballLeague.Controllers
{
    public class IdentityController : ApiController
    {
        private IUsersRepository _repository;

        public IdentityController() : this(null)
        {
        }

        public IdentityController(IUsersRepository repository)
        {
            _repository = repository ?? new UsersRepository();
        }

        [NullObjectActionFilter]
        public User Get()
        {
            var name = User.Identity.Name.Split('\\').Last();
            return _repository.GetUser(name);
        }
    }
}