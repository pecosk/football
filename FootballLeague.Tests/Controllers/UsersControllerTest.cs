using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using FootballLeague.Controllers;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System.Security.Principal;
using System.Threading;
using FootballLeague.Services;

namespace FootballLeague.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTest : ControllerTestBase
    {
        IUsersRepository _repository;
        IUsersADSearcher _adSearcher;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateMock<IUsersRepository>();
            _adSearcher = MockRepository.GenerateMock<IUsersADSearcher>();
        }

        [Test]
        public void Get_ReturnsAllUsers()
        {
            var allUsers = new List<User> { 
                new User(),
                new User()
            };
            _repository.Stub(r => r.GetAllUsers()).Return(allUsers);
            var controller = new UsersController(_repository, _adSearcher);

            IEnumerable<User> result = controller.Get();

            Assert.That(result, Is.EqualTo(allUsers));
        }

        [Test]
        public void GetById_WithExistingId_ReturnsUser()
        {
            var user7 = new User();
            _repository.Stub(r => r.GetUser(7)).Return(user7);
            var controller = new UsersController(_repository, _adSearcher);

            var result = controller.Get(7);

            Assert.That(result, Is.EqualTo(user7));
        }

        [Test]
        public void Post_Always_TriesToInsertUser()
        {
            var mail = "ferko@ferko.sk";
            var first = "Ferdinand";
            var last = "Habsburg";
            _adSearcher.Expect(s => s.LoadUserDetails("Ferko")).Return(new User { Name = "Ferko", Mail = mail, FirstName = first, LastName = last });
            _repository.Expect(r => r.InsertUser(Arg<User>.Matches(u => 
                u.Name == "Ferko"
                && u.Mail == mail
                && u.FirstName == first
                && u.LastName == last)));
            var controller = new UsersController(_repository, _adSearcher);
            MockCurrentUser("Ferko");

            controller.Post();

            _repository.VerifyAllExpectations();
            _adSearcher.VerifyAllExpectations();
        }

        [Test]
        public void Delete_IfCurrentUserExists_DeletesHim()
        {
            var user = new User { Id = 1, Name = "Janko" };
            MockCurrentUser("Janko");
            _repository.Stub(r => r.GetUser(1)).Return(user);
            _repository.Expect(r => r.DeleteUser(1));
            var controller = new UsersController(_repository, _adSearcher);

            controller.Delete(1);

            _repository.VerifyAllExpectations();
        }
    }
}
