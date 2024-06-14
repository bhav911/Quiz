using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Model.CustomModels
{
    public class PaginationModel
    {
        public int TotalPage { get; set; }
        public int CurrentPage { get; set; }
        public List<QuizModel> quizModelList { get; set; }

        public static PaginationModel BindPage(int currentPage, int TotalPage, List<QuizModel> quizModelList)
        {
            int max = 5;
            List<QuizModel> specificPages = quizModelList.Skip((currentPage - 1) * max).Take(max).ToList();
            PaginationModel newPage = new PaginationModel()
            {
                CurrentPage = currentPage,
                quizModelList = specificPages,
                TotalPage = Convert.ToInt32(TotalPage/max) + 1
            };

            return newPage;
        }
    }
}
