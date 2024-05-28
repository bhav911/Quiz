using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace QuizComputation_490_Helper.Helpers
{
    public class AdminCustomAuthenticateJHelperAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return HttpContext.Current.Session["UserRole"].Equals("Admin");
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                {
                    {"controller","User"},
                    {"action", "Unauthorize"},
                    {"role", "user"}
                }
            );
        }
    }
}
