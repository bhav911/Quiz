using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Model.CustomModels
{
    public class QuizModel
    {
        public Nullable<int> QuizID { get; set; }
        
        [Required(ErrorMessage = "Please fill Title field")]
        [MaxLength(length:100, ErrorMessage ="Title too long")]
        public string QuizTitle { get; set; }
        public int FirstQuestionID { get; set; }
        public int LastQuestionID { get; set; }

        [Required(ErrorMessage = "Please fill Description field")]
        public string QuizDescription { get; set; }

        public List<QuestionModel> QuizQuestionList { get; set; }

        public bool isCompleted { get; set; }

        public bool isAttempted { get; set; }
    }
}
