using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuizComputation_490_Helper.Helpers
{
    public class UserCustomAuthenticateHelperAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return HttpContext.Current.Session["Userrole"].Equals("User");
        }

        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new RedirectToRouteResult(
                new System.Web.Routing.RouteValueDictionary()
                {
                    {"controller","Admin" },
                    {"action","Unauthorize" }
                }
            );
        }
    }
}
