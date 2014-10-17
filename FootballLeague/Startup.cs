using Owin; 
using System.Web.Http;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;
using Autofac;
using System.Reflection;
using FootballLeague.Models;
using FootballLeague.Models.Repositories;
using FootballLeague.Services;
using Autofac.Integration.WebApi;
using Nancy;

namespace FootballLeague 
{ 
	public class Startup 
	{ 
		public static void StartupServer()
		{
			string baseAddress = "http://localhost:9000/"; 

			// Start OWIN host 
			using (WebApp.Start<Startup>(url: baseAddress)) 
			{ 
				Console.ReadLine(); 
			} 
		}

		// This code configures Web API. The Startup class is specified as a type
		// parameter in the WebApp.Start method.
		public void Configuration(IAppBuilder appBuilder) 
		{ 
			// Configure Web API for self-host. 
			var config = new HttpConfiguration(); 

			IContainer container = CreateIoCContainer ();
			var dependencyResolver = new AutofacWebApiDependencyResolver(container);
			config.DependencyResolver = dependencyResolver;

			appBuilder.UseAutofacMiddleware(container);
			appBuilder.UseAutofacWebApi(config);

			StaticConfiguration.DisableErrorTraces = false;
			WebApiConfig.Register (config);
			appBuilder.UseWebApi(config)
				.UseNancy ();
		} 

		private IContainer CreateIoCContainer()
		{
			var builder = new ContainerBuilder();
			builder.RegisterApiControllers(Assembly.GetExecutingAssembly()).InstancePerRequest();

			RegisterDependencies(builder);

			return builder.Build();
		}

		private void RegisterDependencies(ContainerBuilder builder)
		{
			builder.Register(c => new FootballContext()).AsSelf().InstancePerRequest();
			builder.Register(c => new UsersRepository(c.Resolve<FootballContext>())).As<IUsersRepository>().InstancePerRequest();
			builder.Register(c => new MatchesRepository(c.Resolve<FootballContext>())).As<IMatchesRepository>().InstancePerRequest();
			builder.Register(c => new UsersADSearcher()).As<IUsersADSearcher>().SingleInstance();
			builder.Register(c => new EmailNotifier()).As<INotifier>().SingleInstance();
		}

	} 
}