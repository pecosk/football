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
            var user = new User { Id = 1, Name = "Ferko" };
            var matches = new List<Match> { new Match { Id = 1, PlannedTime = DateTime.Parse("2014-01-01T12:34"), Creator = user } };
            _context.Matches = MockContextData(_context, c => c.Matches, matches.AsQueryable());
            var repo = new MatchesRepository(_context);
            var newMatchTime = DateTime.Parse("2024-01-01T12:34");
            _context.Matches.Expect(e => e.Add(Arg<Match>.Matches(m => m.PlannedTime == newMatchTime && m.Creator == user && m.Players.Count == 1 && m.Players[0] == user))).Return(null);

            repo.InsertMatch(user, new Match { PlannedTime = newMatchTime });

            _context.Matches.VerifyAllExpectations();
        }

        [Test]
        public void GetPlanned_GetsAllMatchesInFuture()
        {
            var ferko = new User { Id = 1, Name = "Ferko" };
            var longAgo = DateTime.Parse("1896-01-02T01:01");
            var inFuture = DateTime.Parse("2046-01-01T01:01");
            var planned = new List<Match> {
                new Match{ Id = 0, PlannedTime = longAgo, Creator = ferko, Players = new[] { ferko }},
                new Match{ Id = 2, PlannedTime = inFuture, Creator = ferko, Players = new[] { ferko }},
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
                new Match{ Id = 0, PlannedTime = furtherInFuture, Creator = ferko, Players = new[] { ferko }},
                new Match{ Id = 2, PlannedTime = inFuture, Creator = ferko, Players = new[] { ferko }},
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
    }
}
