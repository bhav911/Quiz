using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizComputation_490.Controllers
{
    public class UserController : Controller
    {
        public ActionResult GetAllQuizes()
        {
            return View();
        }
    }
}