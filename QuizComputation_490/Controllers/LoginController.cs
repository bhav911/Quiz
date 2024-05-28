using Newtonsoft.Json;
using OnlineStoreHelper.Helpers;
using QuizComputation_490.Session;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Interface;
using QuizComputation_490_Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace QuizComputation_490.Controllers
{
    public class LoginController : Controller
    {
        private readonly IAdminInterface _admin;
        private readonly IUserInterface _user;


        public LoginController(IAdminInterface admin, IUserInterface user)
        {
            _admin = admin;
            _user = user;
        }
        public ActionResult SignIn()
        {
            Session.Clear();
            return View();
        }

        
        [HttpPost]
        public async Task<ActionResult> SignIn(LoginModel credential)
        {
            if (ModelState.IsValid)
            {
                if (credential.Role == "Admin")
                {
                    string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponsePost("api/LoginAPI/AuthenticateAdmin", JsonConvert.SerializeObject(credential));
                    NewRegistration admin = JsonConvert.DeserializeObject<NewRegistration>(response);
                    if (admin != null)
                    {
                        TempData["success"] = "Successfully Logged In";
                        TempData["Role"] = "Admin";
                        UserSession.UserID = admin.UserID;
                        UserSession.Username = admin.Username;
                        UserSession.UserRole = credential.Role;
                        return RedirectToAction("GetAllQuiz", "Admin");
                    }
                }
                else if (credential.Role == "User")
                {
                    string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponsePost("api/LoginAPI/AuthenticateUser", JsonConvert.SerializeObject(credential));
                    NewRegistration user = JsonConvert.DeserializeObject<NewRegistration>(response);
                    if (user != null)
                    {
                        TempData["success"] = "Successfully Logged In";
                        TempData["Role"] = "User";
                        UserSession.UserID = user.UserID;
                        UserSession.Username = user.Username;
                        UserSession.UserRole = credential.Role;
                        return RedirectToAction("GetAllQuizes", "User");
                    }
                }
                TempData["error"] = "Invalid Credentials";
            }

            return View(credential);
        }


        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> SignUp(NewRegistration newUser)
        {
            if (ModelState.IsValid)
            {
                string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponsePost("api/LoginAPI/SignUp", JsonConvert.SerializeObject(newUser));
                TempData["success"] = "User registered Successfully";
                return RedirectToAction("SignIn");
            }
            TempData["error"] = "Form contains invalid fields";
            return View(newUser);
        }

        public ActionResult SignOut()
        {
            Session.Clear();
            TempData["success"] = "Successfully Logged Out";
            return RedirectToAction("SignIn");
        }

        public ActionResult RedirectToLogin(string err)
        {
            return RedirectToAction("SignIn");
        }
    }
}