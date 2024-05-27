using OnlineStoreHelper.Helpers;
using QuizComputation_490.Session;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Interface;
using QuizComputation_490_Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
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
        public ActionResult SignIn(LoginModel credential)
        {
            if (ModelState.IsValid)
            {
                if (credential.Role == "Admin")
                {
                    Admins admin = _admin.AuthenticateAdmin(credential);
                    if (admin != null)
                    {
                        TempData["Role"] = "Admin";
                        UserSession.UserID = admin.adminID;
                        UserSession.Username = admin.username;
                        UserSession.UserRole = credential.Role;
                        return RedirectToAction("GetAllQuiz", "Admin");
                    }
                }
                else if (credential.Role == "User")
                {
                    Users user = _user.AuthenticateUser(credential);
                    if (user != null)
                    {
                        TempData["Role"] = "User";
                        UserSession.UserID = user.userID;
                        UserSession.Username = user.username;
                        UserSession.UserRole = credential.Role;
                        return RedirectToAction("GetAllQuizes", "User");
                    }
                }
            }

            return View(credential);
        }

        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(NewRegistration newUser)
        {
            if (ModelState.IsValid)
            {                
                Users user = ModelConverter.ConvertNewUserToUser(newUser);
                _user.RegisterUser(user);
                return RedirectToAction("SignIn");
            }
            return View(newUser);
        }

        public ActionResult SignOut()
        {
            Session.Clear();
            return RedirectToAction("SignIn");
        }

        public ActionResult RedirectToLogin(string err)
        {
            return RedirectToAction("SignIn");
        }
    }
}