using System;
using Nancy;

namespace FootballLeague
{
	public class HomeModule : NancyModule
	{
		public HomeModule ()
		{
			Get["/"] = parameters => {
				return View["Index"];
			};
		}
	}
}

