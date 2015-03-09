using Autofac;
using Autofac.Integration.WebApi;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using FootballLeague.Services;
using Newtonsoft.Json;
using System.Data.Entity;
using System.Reflection;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;

namespace FootballLeague
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            Database.SetInitializer<FootballContext>(new DropCreateDatabaseIfModelChanges<FootballContext>());
            GlobalConfiguration.Configuration.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
            GlobalConfiguration.Configuration.DependencyResolver = GetIOCResolver();
            // for now now bundles
            // BundleConfig.RegisterBundles(BundleTable.Bundles);
        }

        private System.Web.Http.Dependencies.IDependencyResolver GetIOCResolver()
        {
            return new AutofacWebApiDependencyResolver(CreateIoCContainer());
        }

        private IContainer CreateIoCContainer()
        {
            var builder = new ContainerBuilder();
            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            RegisterDependencies(builder);

            return builder.Build();
        }

        private void RegisterDependencies(ContainerBuilder builder)
        {
            builder.Register(c => new FootballContext()).AsSelf().InstancePerApiRequest();
            builder.Register(c => new UsersRepository(c.Resolve<FootballContext>())).As<IUsersRepository>().InstancePerApiRequest();
            builder.Register(c => new MatchesRepository(c.Resolve<FootballContext>())).As<IMatchesRepository>().InstancePerApiRequest();
            builder.Register(c => new TournamentRepository(c.Resolve<FootballContext>())).As<ITournamentRepository>().InstancePerApiRequest();
            builder.Register(c => new TournamentTeamRepository(c.Resolve<FootballContext>())).As<ITournamentTeamRepository>().InstancePerApiRequest();
            builder.Register(c => new UsersADSearcher()).As<IUsersADSearcher>().SingleInstance();
            builder.Register(c => new EmailNotifier()).As<INotifier>().SingleInstance();

        }
    }
}