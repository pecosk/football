using System.Collections.Generic;
using NUnit.Framework;
using Rhino.Mocks;
using FootballLeague.Controllers;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using System.Security.Principal;
using System.Threading;

namespace FootballLeague.Tests.Controllers
{
    [TestFixture]
    public class UsersControllerTest
    {
        IUsersRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateMock<IUsersRepository>();
        }

        private void MockCurrentUser(string userName)
        {
            var identity = MockRepository.GenerateMock<IIdentity>();
            identity.Stub(i => i.Name).Return(userName);
            var principal = MockRepository.GenerateMock<IPrincipal>();
            principal.Stub(p => p.Identity).Return(identity);
            Thread.CurrentPrincipal = principal;
        }

        [Test]
        public void Get_ReturnsAllUsers()
        {
            var allUsers = new List<User> { 
                new User(),
                new User()
            };
            _repository.Stub(r => r.GetAllUsers()).Return(allUsers);
            var controller = new UsersController(_repository);

            IEnumerable<User> result = controller.Get();

            Assert.That(result, Is.EqualTo(allUsers));
        }

        [Test]
        public void GetById_WithExistingId_ReturnsUser()
        {
            var user7 = new User();
            _repository.Stub(r => r.GetUser(7)).Return(user7);
            var controller = new UsersController(_repository);

            var result = controller.Get(7);

            Assert.That(result, Is.EqualTo(user7));
        }

        [Test]
        public void Post_WithoutArguments_InsertsCurrentUser()
        {
            _repository.Expect(r => r.InsertUser(Arg<User>.Matches(u => u.Name == "Ferko")));
            var controller = new UsersController(_repository);
            MockCurrentUser("Ferko");

            controller.Post();

            _repository.VerifyAllExpectations();
        }

        [Test]
        public void Delete_IfCurrentUserExists_DeletesHim()
        {
            var user = new User { Name = "Janko" };
            MockCurrentUser("Janko");
            _repository.Stub(r => r.GetUser(1)).Return(user);
            _repository.Expect(r => r.DeleteUser(1));
            var controller = new UsersController(_repository);

            controller.Delete(1);

            _repository.VerifyAllExpectations();
        }
    }
}
