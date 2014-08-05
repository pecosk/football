using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;

namespace FootballLeague.Filters
{
    public class NullObjectActionFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext actionExecutedContext)
        {
            object contentValue = null;
            actionExecutedContext.Response.TryGetContentValue<object>(out contentValue);
            if (contentValue == null)
                actionExecutedContext.Response = actionExecutedContext.Request.CreateErrorResponse(HttpStatusCode.NotFound, "Object not found");
        }
    }
}