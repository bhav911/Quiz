using OnlineStoreHelper.Helpers;
using QuizComputation_490.Session;
using QuizComputation_490_Helper.Helpers;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuizComputation_490.Controllers
{

    [CustomAuthorizeHelper]
    [AdminCustomAuthenticateJHelper]
    public class AdminController : Controller
    {
        private readonly IAdminInterface _admin;

        public AdminController(IAdminInterface admin)
        {
            _admin = admin;
        }

        public ActionResult GetAllQuizes()
        {
            return View();
        }

        public ActionResult CreateQuiz()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateQuiz(QuizModel newQuiz)
        {
            _admin.CreateQuiz(newQuiz, UserSession.UserID);
            return RedirectToAction("GetAllQuizes");
        }
        public ActionResult GetAllQuiz()
        {
            List<Quizzes> quizList = _admin.GetAllQuiz(UserSession.UserID);
            List<QuizModel> quizModelList = ModelConverter.ConvertQuizListToQuizModelList(quizList, UserSession.UserID);
            return View(quizModelList);

        }

        public ActionResult EditQuiz(int quizID)
        {
            Quizzes quiz = _admin.GetQuiz(quizID, UserSession.UserID);
            QuizModel quizModel = ModelConverter.ConvertQuizToQuizModel(quiz, UserSession.UserID);
            return View("CreateQuiz", quizModel);
        }


        [HttpPost]
        public ActionResult EditQuiz(QuizModel updatedQuiz)
        {
            bool status = _admin.UpdateQuiz(updatedQuiz, UserSession.UserID);
            return RedirectToAction("GetAllQuiz");
        }

        public ActionResult DeleteQuiz(int quizID)
        {
            bool status = _admin.DeleteQuiz(quizID);
            return RedirectToAction("GetAllQuiz");
        }

        public ActionResult ShowProfile()
        {
            Admins admin = _admin.GetProfile(UserSession.UserID);
            NewRegistration result = ModelConverter.ConvertAdminToNewAdmin(admin);
            return View(result);
        }

        public ActionResult Unauthorize()
        {
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult EditProfile(NewRegistration updatedInfo)
        {
            _admin.updateProfile(updatedInfo, UserSession.UserID);
            return RedirectToAction("ShowProfile");
        }

        public ActionResult EditProfile()
        {
            Admins admin = _admin.GetProfile(UserSession.UserID);
            NewRegistration result = ModelConverter.ConvertAdminToNewAdmin(admin);
            return View(result);
        }

    }
}