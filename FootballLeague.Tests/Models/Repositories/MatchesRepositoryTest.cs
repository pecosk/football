using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using NUnit.Framework;
using Rhino.Mocks;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace FootballLeague.Tests.Models.Repositories
{
    using FootballLeague.Tests.Data;

    [TestFixture]
    public class MatchesRepositoryTest : RepositoryTestBase
    {
        FootballContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = MockRepository.GenerateMock<FootballContext>();
        }

        [Test]
        public void InsertMatch_InsertMatchWithCreator()
        {
            var user = Users.Ferko;
            var matches = new List<Match> { new Match { Id = 1, PlannedTime = DateTime.Parse("2014-01-01T12:34"), Creator = user, Team1 = TeamData.EmptyTeam, Team2 = TeamData.EmptyTeam } };
            _context.Matches = MockContextData(_context, c => c.Matches, matches.AsQueryable());
            _context.Users = MockContextData(_context, c => c.Users, new List<User>().AsQueryable());
            _context.Teams = MockContextData(_context, c => c.Teams, new List<Team>().AsQueryable());
            var repo = new MatchesRepository(_context);
            var newMatchTime = DateTime.Parse("2024-01-01T12:34");
            _context.Matches
                .Expect(e => e.Add(Arg<Match>
                    .Matches(m => 
                        m.PlannedTime == newMatchTime && 
                        m.Creator.Id == user.Id && 
                        m.Team1.Member1 != null && 
                        m.Team1.Member1.Id == user.Id)))
                .Return(null);
            
            _context.Users.Expect(e => e.Attach(Arg<User>.Is.Same(user))).Return(null);

            repo.InsertMatch(user, new Match { PlannedTime = newMatchTime, Team1 = TeamData.TeamWithFerko, Team2 = TeamData.EmptyTeam });

            _context.Matches.VerifyAllExpectations();
            _context.Users.VerifyAllExpectations();
        }

        [Test]
        public void GetPlanned_GetsAllMatchesInFuture()
        {            
            var longAgo = DateTime.Parse("1896-01-02T01:01");
            var inFuture = DateTime.Parse("2046-01-01T01:01");
            var planned = new List<Match> {
                new Match{ Id = 0, PlannedTime = longAgo, Creator = Users.Ferko, Team1 = TeamData.TeamWithFerko},
                new Match{ Id = 2, PlannedTime = inFuture, Creator = Users.Ferko, Team1 = TeamData.TeamWithFerko},
            };
            _context.Matches = MockContextData(_context, c => c.Matches, planned.AsQueryable());
            var repo = new MatchesRepository(_context);

            var result = repo.GetPlanned();

            Assert.That(result.Count(), Is.EqualTo(1));
            Assert.That(result.First().PlannedTime, Is.EqualTo(inFuture));
        }

        [Test]
        public void GetPlanned_GetsAllMatchesOrderedByPlannedDate()
        {
            var ferko = new User { Id = 1, Name = "Ferko" };
            var furtherInFuture = DateTime.Parse("2046-01-02T01:01");
            var inFuture = DateTime.Parse("2046-01-01T01:01");
            var planned = new List<Match> {
                new Match{ Id = 0, PlannedTime = furtherInFuture, Creator = ferko, Team1 = TeamData.TeamWithFerko},
                new Match{ Id = 2, PlannedTime = inFuture, Creator = ferko, Team1 = TeamData.TeamWithFerko},
            };
            _context.Matches = MockContextData(_context, c => c.Matches, planned.AsQueryable());
            var repo = new MatchesRepository(_context);

            var result = repo.GetPlanned();

            Assert.That(result.Count(), Is.EqualTo(2));
            Assert.That(result.First().PlannedTime, Is.EqualTo(inFuture));
            Assert.That(result.Last().PlannedTime, Is.EqualTo(furtherInFuture));
        }

        [Test]
        public void GetMatch_ForExistingId_ReturnsMatch()
        {
            var match = new Match { Id = 1 };
            var matches = new List<Match> { match };
            _context.Matches = MockContextData(_context, c => c.Matches, matches.AsQueryable());
            var repo = new MatchesRepository(_context);

            var result = repo.GetMatch(1);

            Assert.That(result, Is.EqualTo(match));
        }

        [Test]
        public void GetMatch_ForNonExistingId_ReturnsNull()
        {
            var match = new Match { Id = 1 };
            var matches = new List<Match> { match };
            _context.Matches = MockContextData(_context, c => c.Matches, matches.AsQueryable());
            var repo = new MatchesRepository(_context);

            var result = repo.GetMatch(2);

            Assert.IsNull(result);
        }
        [Test]
        public void AddTeamMember_WhenNotYetInMatch_ThenAddsTeamMember()
        {
            var match = MatchBuilder.Create().WithTeamMember(Users.Ferko).Build();
            var repo = new MatchesRepository(_context);
            _context.Users = MockContextData(_context, c => c.Users, new List<User>().AsQueryable());
            _context.Expect(c => c.SaveChanges()).Return(0);
            _context.Users.Expect(e => e.Attach(Arg<User>.Is.Same(Users.Dano))).Return(null);

            repo.AddMatchParticipantToTeam(Users.Dano, match, match.Team1.Id);

            _context.VerifyAllExpectations();
            _context.Users.VerifyAllExpectations();
            Assert.IsTrue(match.Contains(Users.Dano));
            Assert.IsTrue(match.Contains(Users.Ferko));
        }

        [Test]
        public void AddMatchParticipant_NullPlayers_CreatesOneParticipant()
        {
            var match = MatchBuilder.Create().Build();
            var repo = new MatchesRepository(_context);
            _context.Users = MockContextData(_context, c => c.Users, new List<User>().AsQueryable());
            _context.Expect(c => c.SaveChanges()).Return(0);
            _context.Users.Expect(e => e.Attach(Arg<User>.Is.Same(Users.Ferko))).Return(null);

            repo.AddMatchParticipantToTeam(Users.Ferko, match, match.Team1.Id);

            _context.VerifyAllExpectations();
            _context.Users.VerifyAllExpectations();
            Assert.IsTrue(match.Contains(Users.Ferko));
        }

        [Test]
        public void AddMatchParticipant_PlayerExists_DoesNothing()
        {
            var match = MatchData.MatchWithTwoFullTeams;
            var repo = new MatchesRepository(_context);
            _context.Expect(c => c.SaveChanges()).Repeat.Never();

            repo.AddMatchParticipantToTeam(Users.Ferko, match, match.Team1.Id);

            _context.VerifyAllExpectations();
            Assert.IsTrue(match.Contains(Users.Ferko));
        }

        [Test]
        //TODO wouldn't it make more sense to switch user to other team?
        public void AddMatchParticipant_WhenInOtherTeam_DoesNothing()
        {
            var match = MatchBuilder.Create().WithTeamMember(Users.Ferko).WithTeamMember(Users.Dano).Build();
            match.Team1.SetMember(Users.Dano);
            var repo = new MatchesRepository(_context);
            _context.Expect(c => c.SaveChanges()).Repeat.Never();

            repo.AddMatchParticipantToTeam(Users.Dano, match, match.Team2.Id);

            _context.VerifyAllExpectations();
            Assert.IsFalse(match.Team2.Contains(Users.Dano));
        }

        [Test]
        public void RemoveMatchParticipant_IfNotYetInMatch_DoesNothing()
        {
            var match = MatchBuilder.Create().WithTeamMember(Users.Ferko).Build();
            var repo = new MatchesRepository(_context);
            _context.Expect(c => c.SaveChanges()).Repeat.Never();

            repo.RemoveMatchParticipantFromTeam(Users.Dano, match, match.Team1.Id);

            _context.VerifyAllExpectations();
            Assert.IsFalse(match.Contains(Users.Dano));
        }

        [Test]
        public void RemoveMatchParticipant_NullPlayers_DoesNothing()
        {
            var user = new User();
            var match = MatchBuilder.Create().Build();
            var repo = new MatchesRepository(_context);
            _context.Expect(c => c.SaveChanges()).Repeat.Never();

            repo.RemoveMatchParticipantFromTeam(user, match, match.Team1.Id);

            _context.VerifyAllExpectations();
            Assert.IsNull(match.Team1.Member1);
        }

        [Test]
        public void RemoveMatchParticipant_PlayerExists_RemovesMatchParticipant()
        {
            var match = MatchBuilder.Create().WithTeamMember(Users.Ferko).Build();
            var repo = new MatchesRepository(_context);
            _context.Expect(c => c.SaveChanges()).Return(0);

            repo.RemoveMatchParticipantFromTeam(Users.Ferko, match, match.Team1.Id);

            _context.VerifyAllExpectations();
            Assert.IsFalse(match.Contains(Users.Ferko));
        }
    }
}
