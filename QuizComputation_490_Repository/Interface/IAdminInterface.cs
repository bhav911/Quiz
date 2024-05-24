using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Repository.Interface
{
    public interface IAdminInterface
    {
        Admins AuthenticateAdmin(LoginModel credentials);
        bool CreateQuiz(QuizModel newQuiz, int adminID);
        List<Quizzes> GetAllQuiz(int adminID);
        Quizzes GetQuiz(int adminID);
        bool UpdateQuiz(QuizModel updatedQuiz, int adminID);
        bool DeleteQuiz(int adminID);
    }
}
