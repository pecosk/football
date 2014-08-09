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
            _context.Matches.Expect(e => e.Add(Arg<Match>.Matches(m => m.PlannedTime == newMatchTime && m.Creator == user))).Return(null);

            repo.InsertMatch(user, new Match { PlannedTime = newMatchTime });

            _context.Matches.VerifyAllExpectations();
        }
    }
}
