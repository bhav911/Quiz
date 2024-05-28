using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Model.CustomModels
{
    public class OptionModel
    {
        public int OptionID { get; set; }
        public int QuestionID { get; set; }

        [Required(ErrorMessage = "Please fill option field")]
        [MaxLength(length: 60, ErrorMessage = "Option too long")]
        public string OptionText { get; set; }

        [Required(ErrorMessage ="Select atleast 1 option")]
        public bool IsCorrect { get; set; }
    }
}
