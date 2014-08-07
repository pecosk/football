using FootballLeague.Controllers;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using System;

namespace FootballLeague.Tests.Controllers
{
    [TestFixture]
    public class MatchesControllerTest : ControllerTestBase
    {
        private IMatchesRepository _matchRepo;
        private IUsersRepository _userRepo;

        [SetUp]
        public void SetUp()
        {
            _matchRepo = MockRepository.GenerateMock<IMatchesRepository>();
            _userRepo = MockRepository.GenerateMock<IUsersRepository>();
        }

        [Test]
        public void Put_WithValidMatch_StoresMatch()
        {
            var currentUserName = "Ferko";
            var plannedTime = DateTime.Parse("2026-01-01T12:00");
            var user = new User { Id = 7, Name = currentUserName };
            MockCurrentUser(currentUserName);
            _userRepo.Stub(u => u.GetUser(currentUserName)).Return(user);
            _matchRepo.Expect(r => r.InsertMatch(Arg<User>.Is.Same(user), Arg<Match>.Matches(m => m.PlannedTime == plannedTime)));
            var controller = new MatchesController(_matchRepo, _userRepo);

            controller.Post(new Match { PlannedTime = plannedTime });

            _matchRepo.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void Put_UserUnauthorized_CausesException()
        {
            var currentUserName = "Ferko";
            var plannedTime = DateTime.Parse("2026-01-01T12:00");
            MockCurrentUser(currentUserName);
            _userRepo.Stub(u => u.GetUser(currentUserName)).Return(null);
            _matchRepo.Expect(r => r.InsertMatch(Arg<User>.Is.Anything, Arg<Match>.Is.Anything)).Repeat.Times(0);
            var controller = new MatchesController(_matchRepo, _userRepo);

            controller.Post(new Match { PlannedTime = plannedTime });

            _matchRepo.VerifyAllExpectations();

        }
    }
}
