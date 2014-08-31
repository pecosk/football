using FootballLeague.Controllers;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using FootballLeague.Services;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FootballLeague.Tests.Controllers
{
    [TestFixture]
    public class MatchesControllerTest : ControllerTestBase
    {
        private IMatchesRepository _matchRepo;
        private IUsersRepository _userRepo;
        private INotifier _notifier;

        [SetUp]
        public void SetUp()
        {
            _matchRepo = MockRepository.GenerateMock<IMatchesRepository>();
            _userRepo = MockRepository.GenerateMock<IUsersRepository>();
            _notifier = MockRepository.GenerateMock<INotifier>();
        }

        [Test]
        public void Post_WithValidMatch_StoresMatch()
        {
            var currentUserName = "Ferko";
            var plannedTime = DateTime.Parse("2026-01-01T12:00");
            var user = new User { Id = 7, Name = currentUserName };
            MockCurrentUser(currentUserName);
            _userRepo.Stub(u => u.GetUser(currentUserName)).Return(user);
            _matchRepo.Expect(
                r => r.InsertMatch(Arg<User>.Is.Same(user), Arg<Match>.Matches(m => m.PlannedTime == plannedTime)));
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Post(new Match { PlannedTime = plannedTime });

            _matchRepo.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void Post_UserUnauthorized_CausesException()
        {
            var currentUserName = "Ferko";
            var plannedTime = DateTime.Parse("2026-01-01T12:00");
            MockCurrentUser(currentUserName);
            _userRepo.Stub(u => u.GetUser(currentUserName)).Return(null);
            _matchRepo.Expect(r => r.InsertMatch(Arg<User>.Is.Anything, Arg<Match>.Is.Anything)).Repeat.Times(0);
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Post(new Match { PlannedTime = plannedTime });

            _matchRepo.VerifyAllExpectations();
        }

        [Test]
        public void Post_MatchWithInvitedPlayers_StoresMatchIncludingInvites()
        {
            var currentUserName = "ferko";
            MockCurrentUser(currentUserName);
            var time = DateTime.Parse("2036-01-01T12:00");
            var user = new User { Id = 2, Name = currentUserName };
            var invites = new List<User> { new User(), new User() };
            _userRepo.Stub(u => u.GetUser(currentUserName)).Return(user);
            _matchRepo.Expect(r => r.InsertMatch(Arg<User>.Is.Same(user), Arg<Match>.Matches(m => m.PlannedTime == time)));
            _userRepo.Expect(r => r.UsersExist(Arg<IEnumerable<User>>.Is.Same(invites))).Return(true);
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Post(new Match { PlannedTime = time, Invites = invites });

            _matchRepo.VerifyAllExpectations();
            _userRepo.VerifyAllExpectations();
        }

        [Test]
        public void Post_WhenInvitesFilled_SendsNotificationsToUsers()
        {
            var currentUserName = "ferko";
            MockCurrentUser(currentUserName);
            var time = DateTime.Parse("2036-01-01T12:00");
            var user = new User { Id = 2, Name = currentUserName };
            var invites = new List<User> { new User(), new User() };
            _userRepo.Stub(u => u.GetUser(currentUserName)).Return(user);
            _userRepo.Stub(r => r.UsersExist(Arg<IEnumerable<User>>.Is.Same(invites))).Return(true);
            _notifier.Expect(n => n.Notify(user, invites, time));
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Post(new Match { PlannedTime = time, Invites = invites });

            _notifier.VerifyAllExpectations();
        }

        public void Get_GetsAllPlannedMatches()
        {
            var ferko = new User { Id = 1, Name = "Ferko" };
            var planned = new List<Match> {
                new Match{ Id = 1, PlannedTime = DateTime.Parse("2046-01-02T01:01"), Creator = ferko, Team1 = null },
                new Match{ Id = 2, PlannedTime = DateTime.Parse("2046-01-01T01:01"), Creator = ferko, Team1 = null },
            };
            _matchRepo.Stub(r => r.GetPlanned()).Return(planned);
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            var matches = controller.Get();

            Assert.That(matches, Is.EqualTo(planned));
        }

        [Test]
        public void Put_WithExistingMatchIdWhenUserNotInMatch_AddsCurrentUserToMatch()
        {
            var matchId = 1;
            var teamId = 1;
            var user = new User();
            MockCurrentUser("Ferko");
            var match = new Match { Id = matchId };
            _matchRepo.Stub(r => r.GetMatch(matchId)).Return(match);
            _matchRepo.Stub(r => r.MatchContainsTeam(match, teamId)).Return(true);
            _userRepo.Stub(r => r.GetUser("Ferko")).Return(user);
            _matchRepo.Expect(r => r.AddMatchParticipantToTeam(user, match, teamId));
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Put(matchId, teamId);

            _matchRepo.VerifyAllExpectations();
        }

        [Test]
        public void Put_WithNonexistingMatchId_DoesNothing()
        {
            var matchId = 1;
            var teamId = 1;
            var user = new User();
            MockCurrentUser("Ferko");
            _matchRepo.Stub(r => r.GetMatch(matchId)).Return(null);
            _userRepo.Stub(r => r.GetUser("Ferko")).Return(user);
            _matchRepo.Expect(r => r.AddMatchParticipantToTeam(Arg<User>.Is.Anything, Arg<Match>.Is.Anything, Arg<int>.Is.Anything)).Repeat.Never();
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Put(matchId, teamId);

            _matchRepo.VerifyAllExpectations();
        }

        [Test]
        public void Put_WithNonexistingTeamId_DoesNothing()
        {
            var matchId = 1;
            var teamId = 1;
            var user = new User();
            var match = new Match();
            MockCurrentUser("Ferko");
            _matchRepo.Stub(r => r.GetMatch(matchId)).Return(match);
            _matchRepo.Stub(r => r.MatchContainsTeam(match, teamId)).Return(false);
            _userRepo.Stub(r => r.GetUser("Ferko")).Return(user);
            _matchRepo.Expect(r => r.AddMatchParticipantToTeam(Arg<User>.Is.Anything, Arg<Match>.Is.Anything, Arg<int>.Is.Anything)).Repeat.Never();
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Put(matchId, teamId);

            _matchRepo.VerifyAllExpectations();
        }

        [Test]
        [ExpectedException(typeof(UnauthorizedAccessException))]
        public void Put_WithNonexistingUser_DoesNothing()
        {
            MockCurrentUser("Ferko");
            var teamId = 1;
            _userRepo.Stub(r => r.GetUser("Ferko")).Return(null);
            _matchRepo.Expect(r => r.AddMatchParticipantToTeam(Arg<User>.Is.Anything, Arg<Match>.Is.Anything, Arg<int>.Is.Same(teamId))).Repeat.Never();
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Put(1, teamId);

            _matchRepo.VerifyAllExpectations();
        }

        [Test]
        public void Put_WhenUserInMatch_RemovesCurrentUserFromMatch()
        {
            var matchId = 1;
            var teamId = 1;
            MockCurrentUser("Ferko");
            var ferko = new User();
            var match = new Match { Id = matchId, Team1 = new Team { Member1 = ferko } };
            _matchRepo.Stub(r => r.GetMatch(matchId)).Return(match);
            _matchRepo.Stub(r => r.MatchContainsTeam(match, teamId)).Return(true);
            _userRepo.Stub(r => r.GetUser("Ferko")).Return(ferko);
            _matchRepo.Expect(r => r.RemoveMatchParticipantFromTeam(ferko, match, teamId));            
            var controller = new MatchesController(_matchRepo, _userRepo, _notifier);

            controller.Put(matchId, teamId);

            _matchRepo.VerifyAllExpectations();
        }    
    }
}
