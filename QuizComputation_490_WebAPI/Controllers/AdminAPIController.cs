using OnlineStoreHelper.Helpers;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuizComputation_490_WebAPI.Controllers
{
    public class AdminAPIController : ApiController
    {
        private readonly AdminService _admin = new AdminService();

        [Route("api/AdminAPI/GetAllQuiz")]
        public List<QuizModel> GetAllQuiz(int adminID)
        {
            List<Quizzes> quizList = _admin.GetAllQuiz(adminID);
            List<QuizModel> quizModelList = ModelConverter.ConvertQuizListToQuizModelList(quizList, adminID);
            return quizModelList;
        }

        [HttpGet]
        [Route("api/AdminAPI/EditQuiz")]
        public QuizModel EditQuiz(int quizID, int adminID)
        {
            Quizzes quiz = _admin.GetQuiz(quizID, adminID);
            QuizModel quizModel = ModelConverter.ConvertQuizToQuizModel(quiz, adminID, "Admin");
            return quizModel;
        }

        [HttpGet]
        [Route("api/AdminAPI/DeleteQuiz")]
        public bool DeleteQuiz(int quizID)
        {
            bool status = _admin.DeleteQuiz(quizID);
            return status;
        }

        [HttpGet]
        [Route("api/AdminAPI/ShowProfile")]
        public NewRegistration ShowProfile(int adminID)
        {
            Admins admin = _admin.GetProfile(adminID);
            NewRegistration result = ModelConverter.ConvertAdminToNewAdmin(admin);
            return result;
        }

        [HttpGet]
        [Route("api/AdminAPI/EditProfile")]
        public NewRegistration EditProfile(int adminID)
        {
            Admins admin = _admin.GetProfile(adminID);
            NewRegistration result = ModelConverter.ConvertAdminToNewAdmin(admin);
            return result;
        }


        [HttpPost]
        [Route("api/AdminAPI/CreateQuiz")]
        public bool CreateQuiz(QuizModel newQuiz, int adminID)
        {
            bool status = _admin.CreateQuiz(newQuiz, adminID);
            return status;
        }

        [HttpPost]
        [Route("api/AdminAPI/EditQuiz")]
        public bool EditQuiz(QuizModel updatedQuiz, int adminID)
        {
            bool status = _admin.UpdateQuiz(updatedQuiz, adminID);
            return status;
        }

        [HttpPost]
        [Route("api/AdminAPI/EditProfile")]
        public bool EditProfile(NewRegistration updatedInfo, int adminID)
        {
            bool status = _admin.updateProfile(updatedInfo, adminID);
            return status;
        }
    }
}