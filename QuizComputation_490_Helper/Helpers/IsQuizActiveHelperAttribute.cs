using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuizComputation_490_Helper.Helpers
{
    public class IsQuizActiveHelperAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return HttpContext.Current.Session["IsQuizActive"] == null ? false : (bool)HttpContext.Current.Session["IsQuizActive"];
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary()
                {
                    {"controller","User" },
                    {"action", "Unauthorize"},
                    {"role","user" }
                }
            );
        }
    }
}
