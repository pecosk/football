using Rhino.Mocks;
using System.Security.Principal;
using System.Threading;

namespace FootballLeague.Tests.Controllers
{
    public class ControllerTestBase
    {
        protected void MockCurrentUser(string userName)
        {
            var identity = MockRepository.GenerateMock<IIdentity>();
            identity.Stub(i => i.Name).Return(userName);
            var principal = MockRepository.GenerateMock<IPrincipal>();
            principal.Stub(p => p.Identity).Return(identity);
            Thread.CurrentPrincipal = principal;
        }
    }
}
