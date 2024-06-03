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
            try
            {
                Admins status = db.Admins.Where(o => o.email == credentials.Login_email && o.password == credentials.Login_password).FirstOrDefault();
                return status;
            }
            catch (Exception ex)
            {

                throw ex;
            }
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
            try
            {
                List<Quizzes> quizList = db.Quizzes.Where(q => q.createdBy == adminID).ToList();
                return quizList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Quizzes GetQuiz(int quizID, int adminID)
        {
            try
            {
                Quizzes quiz = db.Quizzes.Where(q => q.createdBy == adminID && q.quizID == quizID).FirstOrDefault();
                return quiz;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool UpdateQuiz(QuizModel updatedQuiz, int adminID)
        {
            try
            {
                Quizzes quiz = db.Quizzes.Where(q => q.quizID == updatedQuiz.QuizID && q.createdBy == adminID).FirstOrDefault();
                quiz.title = updatedQuiz.QuizTitle;
                quiz.description = updatedQuiz.QuizDescription;
                quiz.updatedAt = System.DateTime.Now;
                foreach (QuestionModel question in updatedQuiz.QuizQuestionList)
                {
                    Questions curQue = quiz.Questions.Where(q => q.questionID == question.QuestionID).FirstOrDefault();
                    if (curQue.questionText != question.QuestionText)
                    {
                        curQue.questionText = question.QuestionText;
                        curQue.updatedAT = System.DateTime.Now;
                    }
                    foreach (OptionModel option in question.OptionList)
                    {
                        Options curOption = curQue.Options.Where(o => o.optionID == option.OptionID).FirstOrDefault();
                        if (curOption.optionText != option.OptionText)
                        {
                            curOption.optionText = option.OptionText;
                            curOption.updatedAT = System.DateTime.Now;
                        }
                        if (curOption.isCorrect != option.IsCorrect)
                        {
                            curOption.isCorrect = option.IsCorrect;
                            curOption.updatedAT = System.DateTime.Now;
                        }
                    }
                }
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool DeleteQuiz(int quizID)
        {
            try
            {
                Quizzes quiz = db.Quizzes.Where(q => q.quizID == quizID).FirstOrDefault();
                if (quiz == null || quiz.Results.FirstOrDefault() != null)
                    return false;
                List<Questions> questionList = new List<Questions>();
                foreach (Questions question in quiz.Questions)
                {
                    List<Options> optionList = new List<Options>();
                    foreach(Options option in question.Options)
                    {
                        optionList.Add(option);
                    }
                    db.Options.RemoveRange(optionList);
                    questionList.Add(question);
                }
                db.Questions.RemoveRange(questionList);
                Quizzes removedQuiz = db.Quizzes.Remove(quiz);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public Admins GetProfile(int adminID)
        {
            try
            {
                Admins admin = db.Admins.FirstOrDefault(a => a.adminID == adminID);
                return admin;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool updateProfile(NewRegistration updatedInfo, int userID)
        {
            try
            {
                Admins user = db.Admins.FirstOrDefault(a => a.adminID == userID);
                user.username = updatedInfo.Username;
                user.password = updatedInfo.Password;
                user.email = updatedInfo.Email;
                user.updatedAt = System.DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
