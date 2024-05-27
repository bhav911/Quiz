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
            return View(newQuiz);
        }
        public ActionResult GetAllQuiz()
        {
            List<Quizzes> quizList = _admin.GetAllQuiz(UserSession.UserID);
            List<QuizModel> quizModelList = ModelConverter.ConvertQuizListToQuizModelList(quizList);
            return View(quizModelList);

        }

        public ActionResult EditQuiz(int quizID)
        {
            Quizzes quiz = _admin.GetQuiz(quizID);
            QuizModel quizModel = ModelConverter.ConvertQuizToQuizModel(quiz);
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
    }
}