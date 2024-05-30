using OnlineStoreHelper.Helpers;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Interface;
using QuizComputation_490_Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Results;

namespace QuizComputation_490_WebAPI.Controllers
{
    public class UserAPIController : ApiController
    {
        private readonly UserService _user = new UserService();
        private readonly QuizService _quiz = new QuizService();

        [HttpGet]
        [Route("api/UserAPI/EditProfile")]
        public NewRegistration EditProfile(int userID)
        {
            Users user = _user.GetProfile(userID);
            NewRegistration result = ModelConverter.ConvertUserToNewUser(user);
            _user.GetMultiMedia(result);
            return result;
        }
        
        [Route("api/UserAPI/GetAllQuizes")]
        public List<QuizModel> GetAllQuizes(int userID)
        {
            List<Quizzes> quizList = _quiz.GetAllQuiz();
            List<QuizModel> quizModelsList = ModelConverter.ConvertQuizListToQuizModelList(quizList, userID);
            return quizModelsList;
        }

        [Route("api/UserAPI/GetQuestion")]
        public CompactQuestionModel GetQuestion(int questionID)
        {
            Questions quiz = _quiz.GetQuestion(questionID);
            CompactQuestionModel quizModel = ModelConverter.ConvertQuestionToCompactQuestionModel(quiz);
            return quizModel;
        }

        [HttpGet]
        [Route("api/UserAPI/StartQuiz")]
        public QuizModel StartQuiz(int quizID, int userID)
        {
            Quizzes quiz = _quiz.GetQuiz(quizID);
            QuizModel quizModel = ModelConverter.ConvertQuizToQuizModel(quiz, userID, "user");
            return quizModel;
        }

        [HttpGet]
        [Route("api/UserAPI/SeeAnswers")]
        public List<ResultAnswerModel> SeeAnswers(int quizID, int userID)
        {
            List<ResultAnswerModel> resultAnswerModel = _quiz.GetUserResult(quizID, userID);
            return resultAnswerModel;
        }

        [HttpGet]
        [Route("api/UserAPI/ShowProfile")]
        public NewRegistration ShowProfile(int userID)
        {
            Users user = _user.GetProfile(userID);
            NewRegistration result = ModelConverter.ConvertUserToNewUser(user);
            result.profile = user.UserProfile.FirstOrDefault().profileContent;
            return result;
        }


        [HttpPost]
        [Route("api/UserAPI/EditProfile")]
        public bool EditProfile(NewRegistration updatedInfo, int userID)
        {
            bool status = _user.updateProfile(updatedInfo, userID);
            return status;
        }

        [HttpPost]
        [Route("api/UserAPI/SaveAnswers")]
        public int SaveAnswers(AnswerModel answerModel, int userID)
        {
            int score = _quiz.SubmitAnswer(answerModel, userID);
            return score;
        }
    }
}