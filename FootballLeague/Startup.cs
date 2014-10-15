using Owin; 
using System.Web.Http;
using Microsoft.Owin.Hosting;
using System;
using System.Configuration;

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
			var connectionStrings = ConfigurationManager.ConnectionStrings;
			Console.WriteLine (connectionStrings);

			config.Routes.MapHttpRoute( 
				name: "DefaultApi", 
				routeTemplate: "api/{controller}/{id}", 
				defaults: new { id = RouteParameter.Optional } 
			); 

			appBuilder.UseWebApi(config); 
		} 
	} 
}