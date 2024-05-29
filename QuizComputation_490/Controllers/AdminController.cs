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
using System.Web.Mvc;

namespace QuizComputation_490.Controllers
{

    [CustomAuthorizeHelper]
    [AdminCustomAuthenticateJHelper]
    public class AdminController : Controller
    {
        //private readonly IAdminInterface _admin;

        //public AdminController(IAdminInterface admin)
        //{
        //    _admin = admin;
        //}

        public ActionResult CreateQuiz()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> CreateQuiz(QuizModel newQuiz)
        {
            if (ModelState.IsValid)
            {
                if (!CheckIfOptionIsSelected(newQuiz))
                {
                    TempData["error"] = "You missed to select correct option for some question";
                    return View(newQuiz);
                }
                string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponsePost($"api/AdminAPI/CreateQuiz?adminID={UserSession.UserID}", JsonConvert.SerializeObject(newQuiz));
                TempData["success"] = "Quiz Created Successfully";
                return RedirectToAction("GetAllQuiz");
            }
            TempData["error"] = "Quiz Contains Invalid Fields";
            return View(newQuiz);
        }
        private bool CheckIfOptionIsSelected(QuizModel newQuiz)
        {
            foreach (var question in newQuiz.QuizQuestionList)
            {
                bool flag = false;
                foreach (var option in question.OptionList)
                {
                    if (option.IsCorrect)
                    {
                        flag = true;
                        break;
                    }
                }
                if (!flag)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<ActionResult> GetAllQuiz()
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/AdminAPI/GetAllQuiz?adminID={UserSession.UserID}");
            List<QuizModel> quizModelList = JsonConvert.DeserializeObject<List<QuizModel>>(response);
            return View(quizModelList);

        }

        public async Task<ActionResult> EditQuiz(int quizID)
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/AdminAPI/EditQuiz?adminID={UserSession.UserID}&quizID={quizID}");
            QuizModel quizModel = JsonConvert.DeserializeObject<QuizModel>(response);
            return View("CreateQuiz", quizModel);
        }


        [HttpPost]
        public async Task<ActionResult> EditQuiz(QuizModel updatedQuiz)
        {
            if (ModelState.IsValid)
            {
                string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponsePost($"api/AdminAPI/EditQuiz?adminID={UserSession.UserID}", JsonConvert.SerializeObject(updatedQuiz));
                TempData["success"] = "Quiz Updated Successfully";
                return RedirectToAction("GetAllQuiz");
            }
            TempData["error"] = "Quiz Contains Invalid Fields";
            return View("CreateQuiz", updatedQuiz);
        }

        public async Task<ActionResult> DeleteQuiz(int quizID)
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/AdminAPI/DeleteQuiz?quizID={quizID}");
            TempData["success"] = "Quiz Deleted Successfully";
            return RedirectToAction("GetAllQuiz");
        }

        public async Task<ActionResult> ShowProfile()
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/AdminAPI/ShowProfile?adminID={UserSession.UserID}");
            NewRegistration result = JsonConvert.DeserializeObject<NewRegistration>(response);
            return View(result);
        }

        public ActionResult Unauthorize(string role)
        {
            ViewBag.role = role;
            return View();
        }

        [System.Web.Mvc.HttpPost]
        public async Task<ActionResult> EditProfile(NewRegistration updatedInfo)
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponsePost($"api/AdminAPI/EditProfile?adminID={UserSession.UserID}", JsonConvert.SerializeObject(updatedInfo));
            TempData["success"] = "Profile Updated Successfully";
            return RedirectToAction("ShowProfile");
        }

        public async Task<ActionResult> EditProfile()
        {
            string response = await WebAPICommon.WebApiHelper.HttpClientRequestResponseGet($"api/AdminAPI/EditProfile?adminID={UserSession.UserID}");
            NewRegistration result = JsonConvert.DeserializeObject<NewRegistration>(response);
            return View(result);
        }

    }
}