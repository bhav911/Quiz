using Newtonsoft.Json;
using OnlineStoreHelper.Helpers;
using QuizComputation_490.Session;
using QuizComputation_490_Helper.Helpers;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace QuizComputation_490.Controllers
{
    [CustomAuthorizeHelper]
    [UserCustomAuthenticateHelper]
    public class UserController : Controller
    {
        //private readonly IUserInterface _user;
        //private readonly IQuizInterface _quiz;

        //public UserController(IUserInterface user, IQuizInterface quiz)
        //{
        //    _user = user;
        //    _quiz = quiz;
        //}

        public async Task<ActionResult> EditProfile()
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet("api/UserAPI/EditProfile?userID=" + UserSession.UserID);
            NewRegistration result = JsonConvert.DeserializeObject<NewRegistration>(response);
            return View(result);
        }

        [System.Web.Http.NonAction]
        public string ConvertToStringPhoto(HttpPostedFileBase file)
        {
            if (file.ContentLength <= 0)
                return null;
            var images = new Byte[file.ContentLength - 1];
            file.InputStream.Read(images, 0, images.Length);
            var base64String = Convert.ToBase64String(images, 0, images.Length);
            return base64String;
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EditProfile(NewRegistration updatedInfo)
        {
            updatedInfo.profile = ConvertToStringPhoto(Request.Files["profile"]);
            updatedInfo.aadharCard = ConvertToStringPhoto(Request.Files["aadhar_card"]);
            updatedInfo.Marksheet12 = ConvertToStringPhoto(Request.Files["12Marksheet"]);
            updatedInfo.Marksheet10 = ConvertToStringPhoto(Request.Files["10Marksheet"]);

            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponsePost($"api/UserAPI/EditProfile?userID={UserSession.UserID}", JsonConvert.SerializeObject(updatedInfo));
            TempData["success"] = "Updated Profile Successfully";
            return RedirectToAction("ShowProfile");
        }

        public async Task<ActionResult> GetAllQuizes()
        {
            UserSession.isQuizActive = false;
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet("api/UserAPI/GetAllQuizes?userID=" + UserSession.UserID);
            List<QuizModel> quizModelsList = JsonConvert.DeserializeObject<List<QuizModel>>(response);
            return View(quizModelsList);
        }

        [IsQuizActiveHelper]
        public async Task<PartialViewResult> GetQuestion(int questionID)
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet("api/UserAPI/GetQuestion?questionID=" + questionID);
            CompactQuestionModel quizModel = JsonConvert.DeserializeObject<CompactQuestionModel>(response);
            //quizModel.FirstQuestionID = _quiz.GetFirstQuestionID((int)quiz.quizID);
            //quizModel.LastQuestionID = _quiz.GetLastQuestionID((int)quiz.quizID);
            return PartialView("QuestionBox", quizModel);
        }

        public async Task<ActionResult> StartQuiz(int quizID)
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/UserAPI/StartQuiz?quizID={quizID}&userID={UserSession.UserID}");
            QuizModel quizModel = JsonConvert.DeserializeObject<QuizModel>(response);
            UserSession.isQuizActive = true;
            if (quizModel.isCompleted)
            {
                return RedirectToAction("GetAllQuizes");
            }
            return View(quizModel);
        }

        [System.Web.Mvc.HttpPost]
        public async Task<PartialViewResult> SaveAnswers(List<int> questions, List<int> answers, int quizID)
        {
            if (questions.Count() != 5 || answers.Count() != 5)
            {
                throw new Exception();
            }
            AnswerModel answerModel = new AnswerModel()
            {
                Questions = questions,
                Answers = answers,
                quizID = quizID
            };
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponsePost($"api/UserAPI/SaveAnswers?userID={UserSession.UserID}", JsonConvert.SerializeObject(answerModel));
            int score = Convert.ToInt32(response);
            UserSession.isQuizActive = false;
            ResultModel resultModel = new ResultModel()
            {
                quizID = quizID,
                score = score,
                TotalQuestions = 5
            };
            //for-scalability of question numbers
            //resultModel.TotalQuestions = _quiz.GetTotalQuestions(quizID);
            TempData["success"] = "Successfully submited your response";
            return PartialView("ResultBox", resultModel);
        }

        public async Task<PartialViewResult> SeeAnswers(int quizID)
        {
            UserSession.isQuizActive = false;
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/UserAPI/SeeAnswers?quizID={quizID}&userID={UserSession.UserID}");
            List<ResultAnswerModel> resultAnswerModel = JsonConvert.DeserializeObject<List<ResultAnswerModel>>(response);
            return PartialView("ShowAnswers", resultAnswerModel);
        }

        public ActionResult GetResult(int quizID)
        {
            UserSession.isQuizActive = false;
            ViewBag.quizID = quizID;
            return View();
        }

        public async Task<ActionResult> ShowProfile()
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/UserAPI/ShowProfile?userID={UserSession.UserID}");
            NewRegistration result = JsonConvert.DeserializeObject<NewRegistration>(response);
            return View(result);
        }

        public ActionResult Unauthorize(string role)
        {
            ViewBag.role = role;
            return View();
        }

        public async Task<ActionResult> DeleteProfile(int userID)
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/UserAPI/DeleteProfile?userID={userID}");
            HttpContext.Session.Clear();
            TempData["success"] = "Account Deleted Successfully";
            return RedirectToAction("SignIn", "Login");
        }
    }
}