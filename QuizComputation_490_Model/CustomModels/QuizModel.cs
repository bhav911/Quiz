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
        public int QuizID { get; set; }
        
        [Required]
        [MaxLength(length:100, ErrorMessage ="Title too long")]
        public string QuizTitle { get; set; }

        [Required]
        [MaxLength(length:250, ErrorMessage ="Description too long")]
        public string QuizDescription { get; set; }
        public List<QuestionModel> QuizQuestionList { get; set; }
    }
}
