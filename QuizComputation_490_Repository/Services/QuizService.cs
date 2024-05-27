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
            List<Quizzes> quizModelList = db.Quizzes.ToList();
            return quizModelList;
        }

        public Quizzes GetQuiz(int quizID)
        {
            Quizzes quiz = db.Quizzes.Where(q => q.quizID == quizID).FirstOrDefault();
            return quiz;
        }

        public Questions GetQuestion(int questionID)
        {
            Questions question = db.Questions.Where(q => q.questionID == questionID).FirstOrDefault();
            return question;
        }

        public int GetFirstQuestionID(int quizID)
        {
            Quizzes quiz = db.Quizzes.Where(q => q.quizID == quizID).FirstOrDefault();
            int queID = quiz.Questions.OrderBy(q => q.questionID).FirstOrDefault().questionID;
            return quizID;
        }

        public int GetLastQuestionID(int quizID)
        {
            Quizzes quiz = db.Quizzes.Where(q => q.quizID == quizID).FirstOrDefault();
            int queID = quiz.Questions.OrderBy(q => q.questionID).LastOrDefault().questionID;
            return queID;
        }

        public int SubmitAnswer(AnswerModel answerModel, int userID)
        {
            SqlConnection con = new SqlConnection("Data Source=182.70.118.201,1580;Initial Catalog=QuizComputation_490;user id=sa;password=sit@123;");
            con.Open();


            for (int it = 0; it < 5; it++)
            {
                using (SqlCommand cmd = new SqlCommand("SubmitUserAnswer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@quizID", answerModel.quizID);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@questionID", answerModel.Questions[it]);
                    cmd.Parameters.AddWithValue("@optionOffset", answerModel.Answers[it]);

                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {}
                    }
                }
            }

            int score = 0;

            using (SqlCommand cmd = new SqlCommand("InsertUserScore", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@quizID", answerModel.quizID);
                cmd.Parameters.AddWithValue("@userID", userID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        score = (int)reader["score"];
                    }
                }
            }

            con.Close();
            return score;
        }

        //public ResultModel GetResult(int userID, int quizID)
        //{
        //    int totalQuestion = db.Questions.Where(q => q.quizID == quizID).Count();
        //    int count = db.Results.Where(q => q.quizID = quizID && (int)q.userID = userID);

        //    ResultModel resultModel = new ResultModel
        //    {
        //        quizID = quizID,
        //        CorrectAnswers = count,
        //        TotalQuestions = totalQuestion
        //    };

        //    return resultModel;
        //}

        public int GetTotalQuestions(int quizID)
        {
            int totalQuestions = db.Questions.Where(q => q.quizID == quizID).Count();
            return totalQuestions;
        }

        public List<ResultAnswerModel> GetUserResult(int quizID,  int userID)
        {
            List<ResultAnswerModel> resultAnswerModelList = new List<ResultAnswerModel>();

            SqlConnection con = new SqlConnection("Data Source=182.70.118.201,1580;Initial Catalog=QuizComputation_490;user id=sa;password=sit@123;");
            con.Open();

            using (SqlCommand cmd = new SqlCommand("GetUserResult", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@quizID", quizID);
                cmd.Parameters.AddWithValue("@userID", userID);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        ResultAnswerModel resultAnswerModel = new ResultAnswerModel()
                        {
                            correctOption = ((int)reader["Correct Option"] - 1) % 4,
                            UserSelectedOption = ((int)reader["User Selected Option"] - 1) % 4,
                            optionsText = new List<string>()
                        };
                        int questionID = (int)reader["questionID"];
                        Questions question = db.Questions.Where(q => q.questionID == questionID).FirstOrDefault();
                        resultAnswerModel.questionText = question.questionText;
                        foreach(Options option in question.Options)
                        {
                            resultAnswerModel.optionsText.Add(option.optionText);
                        }
                        resultAnswerModelList.Add(resultAnswerModel);
                    }
                }
            }

            con.Close();
            return resultAnswerModelList;
        }
    }
}
