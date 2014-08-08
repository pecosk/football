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
    public class MatchesRepositoryTest
    {
        FootballContext _context;

        [SetUp]
        public void SetUp()
        {
            _context = MockRepository.GenerateMock<FootballContext>();
        }

        private IDbSet<T> MockContextData<T>(FootballContext context, Func<FootballContext, IDbSet<T>> dataSelector, IQueryable<T> mockData) where T : class
        {
            var mockSet = MockRepository.GenerateMock<IDbSet<T>, IQueryable>();
            mockSet.Stub(m => m.Provider).Return(mockData.Provider);
            mockSet.Stub(m => m.Expression).Return(mockData.Expression);
            mockSet.Stub(m => m.ElementType).Return(mockData.ElementType);
            mockSet.Stub(m => m.GetEnumerator()).Return(mockData.GetEnumerator());
            context.Stub(x => dataSelector(x)).PropertyBehavior();
            return mockSet;
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
