using FootballLeague.Controllers;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using NUnit.Framework;
using Rhino.Mocks;

namespace FootballLeague.Tests.Controllers
{
    [TestFixture]
    public class IdentityControllerTest : ControllerTestBase
    {
        IUsersRepository _repository;

        [SetUp]
        public void SetUp()
        {
            _repository = MockRepository.GenerateStub<IUsersRepository>();
        }

        [Test]
        public void Get_ForExistingActiveAuthenticatedUser_ReturnsUser()
        {
            var user = new User();
            MockCurrentUser("Ferko");
            _repository.Stub(r => r.GetUser("Ferko")).Return(user);
            var controller = new IdentityController(_repository);

            var authenticatedUser = controller.Get();

            Assert.That(authenticatedUser, Is.EqualTo(user));
        }

        [Test]
        public void Get_ForNonExistingAuthenticatedUser_ReturnsNull()
        {
            MockCurrentUser("Ferko");
            _repository.Stub(r => r.GetUser("Ferko")).Return(null);
            var controller = new IdentityController(_repository);

            var authenticatedUser = controller.Get();

            Assert.IsNull(authenticatedUser);
        }

        [Test]
        public void Get_ForExistingInctiveAuthenticatedUser_ReturnsNull()
        {
            var user = new User { Inactive = true };
            MockCurrentUser("Ferko");
            _repository.Stub(r => r.GetUser("Ferko")).Return(user);
            var controller = new IdentityController(_repository);

            var authenticatedUser = controller.Get();

            Assert.IsNull(authenticatedUser);
        }
    }
}
