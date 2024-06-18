using QuizComputation_490.Common;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Repository.Services
{
    public class QuizService : IQuizInterface
    {
        private readonly QuizComputation_490Entities db = new QuizComputation_490Entities();
        public List<Quizzes> GetAllQuiz()
        {
            try
            {
                List<Quizzes> quizModelList = db.Quizzes.ToList();
                return quizModelList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Quizzes GetQuiz(int quizID)
        {
            try
            {
                Quizzes quiz = db.Quizzes.Where(q => q.quizID == quizID).FirstOrDefault();
                return quiz;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Questions GetQuestion(int questionID)
        {
            try
            {
                Questions question = db.Questions.Where(q => q.questionID == questionID).FirstOrDefault();
                return question;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetFirstQuestionID(int quizID)
        {
            try
            {
                Quizzes quiz = db.Quizzes.Where(q => q.quizID == quizID).FirstOrDefault();
                int queID = quiz.Questions.OrderBy(q => q.questionID).FirstOrDefault().questionID;
                return queID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int GetLastQuestionID(int quizID)
        {
            try
            {
                Quizzes quiz = db.Quizzes.Where(q => q.quizID == quizID).FirstOrDefault();
                int queID = quiz.Questions.OrderBy(q => q.questionID).LastOrDefault().questionID;
                return queID;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public int SubmitAnswer(AnswerModel answerModel, int userID)
        {
            try
            {
                DataTable resultTable;
                Dictionary<string, object> kvp;
                for (int it = 0; it < answerModel.Questions.Count() ; it++)
                {
                    kvp = new Dictionary<string, object>()
                    {
                        { "@quizID", answerModel.quizID},
                        {"@userID", userID },
                        { "@questionID", answerModel.Questions[it]},
                        {"@optionOffset", answerModel.Answers[it]}
                    };
                    resultTable = SqlSPHelper.SqlSPConnector("SubmitUserAnswer", kvp);
                }

                int score = 0;
                kvp = new Dictionary<string, object>()
                {
                    { "@quizID", answerModel.quizID},
                    {"@userID", userID }
                };

                resultTable = SqlSPHelper.SqlSPConnector("InsertUserScore", kvp);
                foreach (DataRow row in resultTable.Rows)
                {
                    foreach (DataColumn column in resultTable.Columns)
                    {
                        score = (int)row[column];
                    }
                }
                return score;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public int GetTotalQuestions(int quizID)
        {
            try
            {
                int totalQuestions = db.Questions.Where(q => q.quizID == quizID).Count();
                return totalQuestions;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<ResultAnswerModel> GetUserResult(int quizID, int userID)
        {
            try
            {
                List<ResultAnswerModel> resultAnswerModelList = new List<ResultAnswerModel>();

                Dictionary<string, object> kvp = new Dictionary<string, object>()
                {
                    { "@quizID", quizID },
                    { "@userID", userID }
                };
                DataTable resultTable = SqlSPHelper.SqlSPConnector("GetUserResult", kvp);

                foreach(DataRow row in resultTable.Rows)
                {
                    ResultAnswerModel resultAnswerModel = new ResultAnswerModel()
                    {
                        correctOption = ((int)row["Correct Option"] - 1) % 4,
                        UserSelectedOption = ((int)row["User Selected Option"] - 1) % 4,
                        optionsText = new List<string>()
                    };
                    int questionID = (int)row["questionID"];
                    Questions question = db.Questions.Where(q => q.questionID == questionID).FirstOrDefault();
                    resultAnswerModel.questionText = question.questionText;
                    foreach (Options option in question.Options)
                    {
                        resultAnswerModel.optionsText.Add(option.optionText);
                    }
                    resultAnswerModelList.Add(resultAnswerModel);
                }
                return resultAnswerModelList;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }
}
