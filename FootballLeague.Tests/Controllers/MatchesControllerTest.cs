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
            _matchRepo.Expect(
                r => r.InsertMatch(Arg<User>.Is.Same(user), Arg<Match>.Matches(m => m.PlannedTime == plannedTime)));
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

        //public void Get_GetsAllPlannedMatches()
        //{
        //    var ferko = new User { Id = 1, Name = "Ferko" };
        //    var planned = new List<Match> {
        //        new Match{ Id = 1, PlannedTime = DateTime.Parse("2046-01-02T01:01"), Creator = ferko, Team1 = TeamBuilder.TeamWithFerko},
        //        new Match{ Id = 2, PlannedTime = DateTime.Parse("2046-01-01T01:01"), Creator = ferko, Team1 = TeamBuilder.TeamWithFerko},
        //    };
        //    _matchRepo.Stub(r => r.GetPlanned()).Return(planned);
        //    var controller = new MatchesController(_matchRepo, _userRepo, _teamRepo);

        //    var matches = controller.Get();

        //    Assert.That(matches, Is.EqualTo(planned));
        //}

        //[Test]
        //public void Put_WithExistingMatchIdWhenUserNotInMatch_AddsCurrentUserToMatch()
        //{
        //    var matchId = 1;
        //    var user = new User();
        //    MockCurrentUser("Ferko");
        //    var match = new Match { Id = matchId };
        //    _matchRepo.Stub(r => r.GetMatch(matchId)).Return(match);
        //    _userRepo.Stub(r => r.GetUser("Ferko")).Return(user);
        //    _matchRepo.Expect(r => r.AddMatchParticipant(user, match.Team1));
        //    var controller = new MatchesController(_matchRepo, _userRepo, _teamRepo);

        //    controller.Put(matchId);

        //    _matchRepo.VerifyAllExpectations();
        //}

        //[Test]
        //public void Put_WithNonexistingMatchId_DoesNothing()
        //{
        //    var matchId = 1;
        //    var user = new User();
        //    MockCurrentUser("Ferko");
        //    _matchRepo.Stub(r => r.GetMatch(matchId)).Return(null);
        //    _userRepo.Stub(r => r.GetUser("Ferko")).Return(user);
        //    _matchRepo.Expect(r => r.AddMatchParticipant(Arg<User>.Is.Anything, Arg<Team>.Is.Anything)).Repeat.Never();
        //    var controller = new MatchesController(_matchRepo, _userRepo, _teamRepo);

        //    controller.Put(matchId);

        //    _matchRepo.VerifyAllExpectations();
        //}

        //[Test]
        //public void Put_WithNonexistingUser_DoesNothing()
        //{
        //    MockCurrentUser("Ferko");
        //    _userRepo.Stub(r => r.GetUser("Ferko")).Return(null);
        //    _matchRepo.Expect(r => r.AddMatchParticipant(Arg<User>.Is.Anything, Arg<Team>.Is.Anything)).Repeat.Never();
        //    var controller = new MatchesController(_matchRepo, _userRepo, _teamRepo);

        //    controller.Put(1);

        //    _matchRepo.VerifyAllExpectations();
        //}

        //[Test]
        //public void Put_WhenUserInMatch_AddsCurrentUserToMatch()
        //{
        //    var teamId = 1;
        //    MockCurrentUser("Ferko");
        //    var match = new Match { Id = matchId, Team1 = TeamBuilder.TeamWithFerko};
        //    _teamRepo.Expect(r => r.)
        //    _matchRepo.Stub(r => r.GetMatch(matchId)).Return(match);
        //    _userRepo.Stub(r => r.GetUser("Ferko")).Return(Users.Ferko);
        //    _matchRepo.Expect(r => r.RemoveMatchParticipant(Users.Ferko, match.Team1));            
        //    var controller = new MatchesController(_matchRepo, _userRepo, _teamRepo);

        //    controller.Put(matchId);

        //    _matchRepo.VerifyAllExpectations();
        //}    
    }
}
