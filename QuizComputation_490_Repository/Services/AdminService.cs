using OnlineStoreHelper.Helpers;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Repository.Services
{
    public class AdminService : IAdminInterface
    {
        private readonly QuizComputation_490Entities db = new QuizComputation_490Entities();
        
        public Admins AuthenticateAdmin(LoginModel credentials)
        {
            Admins status = db.Admins.Where(o => o.email == credentials.Login_email && o.password == credentials.Login_password).FirstOrDefault();
            return status;
        }

        public bool CreateQuiz(QuizModel newQuiz, int adminID)
        {
            try
            {
                Quizzes quiz = ModelConverter.ConvertQuizModelToQuiz(newQuiz, adminID);
                Quizzes returnedQuiz = db.Quizzes.Add(quiz);
                db.SaveChanges();

                for (int que = 0; que < newQuiz.QuizQuestionList.Count(); que++)
                {
                    Questions question = new Questions()
                    {
                        createdAt = System.DateTime.Now,
                        quizID = returnedQuiz.quizID,
                        questionText = newQuiz.QuizQuestionList[que].QuestionText
                    };

                    Questions returnedQuestion = db.Questions.Add(question);
                    db.SaveChanges();

                    foreach (OptionModel option in newQuiz.QuizQuestionList[que].OptionList)
                    {
                        Options newOption = new Options()
                        {
                            questionID = returnedQuestion.questionID,
                            optionText = option.OptionText,
                            createdAt = System.DateTime.Now,
                            isCorrect = option.IsCorrect
                        };

                        db.Options.Add(newOption);
                    }
                }

                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }

        public List<Quizzes> GetAllQuiz(int adminID)
        {
            List<Quizzes> quizList = db.Quizzes.Where(q => q.createdBy == adminID).ToList();
            return quizList;
        }

        public Quizzes GetQuiz(int adminID)
        {
            Quizzes quiz = db.Quizzes.Where(q => q.createdBy == adminID).FirstOrDefault();
            return quiz;
        }
    }
}
