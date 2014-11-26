using System.Web.Http.Filters;
using System;

namespace FootballLeague
{
	public class NoCacheHeaderActionFilter : ActionFilterAttribute
	{
		public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
		{
			//actionExecutedContext.Response.Content.Headers.Expires = DateTimeOffset.Now;
		}
	}}

