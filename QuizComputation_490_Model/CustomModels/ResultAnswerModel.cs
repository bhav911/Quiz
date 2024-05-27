using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Model.CustomModels
{
    public class ResultAnswerModel
    {
        public string questionText { get; set; }
        public List<string> optionsText { get; set; }
        public  int correctOption { get; set; }
        public int UserSelectedOption { get; set; }
    }
}
