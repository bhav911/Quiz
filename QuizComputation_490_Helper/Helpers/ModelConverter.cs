using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OnlineStoreHelper.Helpers
{
    public class ModelConverter
    {

        public static Users ConvertNewUserToUser(NewRegistration userModel)
        {
            Users user = new Users()
            {
                username = userModel.Username,
                email = userModel.Email,
                password = userModel.Password,
                createdAt = System.DateTime.Now
            };

            return user;
        }

        public static NewRegistration ConvertUserToNewUser(Users user)
        {
            NewRegistration newRegistration = new NewRegistration()
            {
                Userrole = "User",
                UserID = user.userID,
                Email = user.email,
                Username = user.username,
                Password = user.password
            };

            return newRegistration;
        }

        public static NewRegistration ConvertAdminToNewUser(Admins user)
        {
            NewRegistration newRegistration = new NewRegistration()
            {
                Userrole = "User",
                UserID = user.adminID,
                Email = user.email,
                Username = user.username,
                Password = user.password
            };

            return newRegistration;
        }

        public static NewRegistration ConvertAdminToNewAdmin(Admins admin)
        {
            NewRegistration newRegistration = new NewRegistration()
            {
                Userrole = "Admin",
                UserID = admin.adminID,
                Email = admin.email,
                Username = admin.username,
                Password = admin.password
            };

            return newRegistration;
        }

        public static Quizzes ConvertQuizModelToQuiz(QuizModel quizModel, int adminID)
        {
            Quizzes quiz = new Quizzes()
            {
                createdBy = adminID,
                createdAt = System.DateTime.Now,
                description = quizModel.QuizDescription,
                title = quizModel.QuizTitle
            };
            return quiz;
        }

        public static List<QuizModel> ConvertQuizListToQuizModelList(List<Quizzes> quizList, int userID)
        {
            List<QuizModel> quizModelList = new List<QuizModel>();

            foreach (Quizzes quiz in quizList)
            {
                QuizModel quizModel = new QuizModel()
                {
                    QuizTitle = quiz.title,
                    QuizDescription = quiz.description,
                    QuizID = quiz.quizID,
                    isCompleted = quiz.Results.Where(q => q.quizID == quiz.quizID && q.userID == userID).FirstOrDefault() != null
                };

                quizModelList.Add(quizModel);
            }

            return quizModelList;
        }

        public static QuizModel ConvertQuizToQuizModel(Quizzes quiz, int userID, string role)
        {
            QuizModel quizModel = new QuizModel()
            {
                QuizID = quiz.quizID,
                QuizTitle = quiz.title,
                QuizDescription = quiz.description,
                FirstQuestionID = quiz.Questions.OrderBy(q => q.questionID).FirstOrDefault().questionID ,
                isCompleted = quiz.Results.Where(q => q.quizID == quiz.quizID && q.userID == userID).FirstOrDefault() != null
            };
            if(role != "user")
                quizModel.QuizQuestionList = ConvertQuestionListToQuestionModelList(quiz.Questions.ToList());
            return quizModel;
        }

        public static List<QuestionModel> ConvertQuestionListToQuestionModelList(List<Questions> questionList)
        {
            List<QuestionModel> questionModelList = new List<QuestionModel>();
            foreach (Questions question in questionList)
            {
                QuestionModel questionModel = new QuestionModel()
                {
                    QuestionID = question.questionID,
                    QuestionText = question.questionText
                };

                questionModel.OptionList = ConvertOptionListToOptionModelList(question.Options.ToList());

                questionModelList.Add(questionModel);
            }

            return questionModelList;
        }

        public static QuestionModel ConvertQuestionToQuestionModel(Questions question)
        {
            QuestionModel questionModel = new QuestionModel
            {
                QuestionID =question.questionID,
                QuestionText= question.questionText
            };

            foreach(Options option in question.Options)
            {
                OptionModel newOption = new OptionModel()
                {
                    OptionText = option.optionText,
                    OptionID = option.optionID
                };
                questionModel.OptionList.Add(newOption);
            }
            
            return questionModel;
        }

        public static CompactQuestionModel ConvertQuestionToCompactQuestionModel(Questions question)
        {
            CompactQuestionModel questionModel = new CompactQuestionModel
            {
                QuestionID = question.questionID,
                QuestionText = question.questionText,
                QuizID = (int)question.quizID
            };

            foreach (Options option in question.Options)
            {
                questionModel.OptionList.Add(option.optionText);
            }

            return questionModel;
        }

        public static List<OptionModel> ConvertOptionListToOptionModelList(List<Options> optionList)
        {
            List<OptionModel> optionModelList = new List<OptionModel>();

            foreach (Options option in optionList)
            {
                OptionModel optionModel = new OptionModel()
                {
                    IsCorrect = (bool)option.isCorrect,
                    OptionID = option.optionID,
                    OptionText = option.optionText,
                    QuestionID = (int)option.questionID
                };
                optionModelList.Add(optionModel);
            }

            return optionModelList;
        }
    }
}
