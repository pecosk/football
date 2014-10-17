using System;
using Nancy;
using Nancy.Conventions;

namespace FootballLeague
{
	public class ApplicationBootstrapper : DefaultNancyBootstrapper
	{
		protected override void ConfigureConventions(NancyConventions nancyConventions)
		{
			nancyConventions.StaticContentsConventions.Add(
				StaticContentConventionBuilder.AddDirectory("Scripts", @"Scripts")
			);
			nancyConventions.StaticContentsConventions.Add(
				StaticContentConventionBuilder.AddDirectory("app", @"app")
			);
			nancyConventions.StaticContentsConventions.Add(
				StaticContentConventionBuilder.AddDirectory("fonts", @"fonts")
			);
			base.ConfigureConventions(nancyConventions);
		}
	}
}

