using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Repository.Interface
{
    public interface IQuizInterface
    {
        Questions GetQuestion(int questionID);
        List<Quizzes> GetAllQuiz();
        Quizzes GetQuiz(int quizID);
        int GetFirstQuestionID(int quizID);
        int GetLastQuestionID(int quizID);
        int SubmitAnswer(AnswerModel answerModel, int userID);
        //ResultModel GetResult(int userID, int quizID);
        int GetTotalQuestions(int quizID);
        List<ResultAnswerModel> GetUserResult(int questionID, int userID);
    }
}
