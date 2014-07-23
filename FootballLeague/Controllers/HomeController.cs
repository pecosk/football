using FootballLeague.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FootballLeague.Controllers
{
    public class HomeController : Controller
    {
        private UsersRepository _repository;

        public HomeController()
        {
            _repository = new UsersRepository();
        }

        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
    }
}
