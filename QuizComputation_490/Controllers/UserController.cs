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
using System.Web.Http;
using System.Web.Mvc;

namespace QuizComputation_490.Controllers
{
    [CustomAuthorizeHelper]
    [UserCustomAuthenticateHelper]
    public class UserController : Controller
    {
        private readonly IUserInterface _user;
        private readonly IQuizInterface _quiz;

        public UserController(IUserInterface user, IQuizInterface quiz)
        {
            _user = user;
            _quiz = quiz;
        }

        public ActionResult EditProfile()
        {
            Users user = _user.GetProfile(UserSession.UserID);
            NewRegistration result = ModelConverter.ConvertUserToNewUser(user);
            return View(result);
        }

        [System.Web.Mvc.HttpPost]
        public ActionResult EditProfile(NewRegistration updatedInfo)
        {
            _user.updateProfile(updatedInfo, UserSession.UserID);
            return RedirectToAction("ShowProfile");
        }

        public ActionResult GetAllQuizes()
        {
            List<Quizzes> quizList = _quiz.GetAllQuiz();
            List<QuizModel> quizModelsList = ModelConverter.ConvertQuizListToQuizModelList(quizList, UserSession.UserID);
            return View(quizModelsList);
        }

        public PartialViewResult GetQuestion(int questionID)
        {
            Questions quiz = _quiz.GetQuestion(questionID);
            CompactQuestionModel quizModel = ModelConverter.ConvertQuestionToCompactQuestionModel(quiz);
            //quizModel.FirstQuestionID = _quiz.GetFirstQuestionID((int)quiz.quizID);
            //quizModel.LastQuestionID = _quiz.GetLastQuestionID((int)quiz.quizID);
            return PartialView("QuestionBox", quizModel);
        }
        public ActionResult StartQuiz(int quizID)
        {
            Quizzes quiz = _quiz.GetQuiz(quizID);
            QuizModel quizModel = ModelConverter.ConvertQuizToQuizModel(quiz, UserSession.UserID);
            if (quizModel.isCompleted)
            {
                return RedirectToAction("GetAllQuizes");
            }
            return View(quizModel);
        }

        [System.Web.Mvc.HttpPost]
        public PartialViewResult SaveAnswers(List<int> questions, List<int> answers, int quizID)
        {
            AnswerModel answerModel = new AnswerModel()
            {
                Questions = questions,
                Answers = answers,
                quizID = quizID
            };
            int score = _quiz.SubmitAnswer(answerModel, UserSession.UserID);
            ResultModel resultModel = new ResultModel()
            {
                quizID = quizID,
                score = score,
                TotalQuestions = 5
            };
            //for-scalability of question numbers
            //resultModel.TotalQuestions = _quiz.GetTotalQuestions(quizID);
            return PartialView("ResultBox", resultModel);
        }

        public PartialViewResult SeeAnswers(int quizID)
        {
            List<ResultAnswerModel> resultAnswerModel = _quiz.GetUserResult(quizID, UserSession.UserID);
            return PartialView("ShowAnswers", resultAnswerModel);
        }

        public ActionResult GetResult(int quizID)
        {
            ViewBag.quizID = quizID;
            return View();
        }

        public ActionResult ShowProfile()
        {
            try
            {
                Users user = _user.GetProfile(UserSession.UserID);
                NewRegistration result = ModelConverter.ConvertUserToNewUser(user);
                return View(result);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public ActionResult Unauthorize()
        {
            return View();
        }
    }
}