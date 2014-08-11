using FootballLeague.Models;
using Rhino.Mocks;
using System;
using System.Data.Entity;
using System.Linq;

namespace FootballLeague.Tests.Models.Repositories
{
    public class RepositoryTestBase
    {
        protected IDbSet<T> MockContextData<T>(FootballContext context, Func<FootballContext, IDbSet<T>> dataSelector, IQueryable<T> mockData) where T : class
        {
            var mockSet = MockRepository.GenerateMock<IDbSet<T>, IQueryable>();
            mockSet.Stub(m => m.Provider).Return(mockData.Provider);
            mockSet.Stub(m => m.Expression).Return(mockData.Expression);
            mockSet.Stub(m => m.ElementType).Return(mockData.ElementType);
            mockSet.Stub(m => m.GetEnumerator()).Return(mockData.GetEnumerator());
            context.Stub(x => dataSelector(x)).PropertyBehavior();
            return mockSet;
        }
    }
}
