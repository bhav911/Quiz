using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Model.CustomModels
{
    public class QuestionModel
    {
        public int QuestionID { get; set; }
        public int QuizID { get; set; }

        [Required]
        [MaxLength(length: 200, ErrorMessage ="Question too long")]
        public string QuestionText { get; set; }

        public int FirstQuestionID { get; set; }
        public int LastQuestionID { get; set; }

        public List<OptionModel> OptionList { get; set; }

        public QuestionModel()
        {
            OptionList = new List<OptionModel>();
        }

    }
}
