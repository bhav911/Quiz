//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace QuizComputation_490_Model.Context
{
    using System;
    using System.Collections.Generic;
    
    public partial class UserAnswers
    {
        public int answerID { get; set; }
        public Nullable<int> userID { get; set; }
        public Nullable<int> quizID { get; set; }
        public Nullable<int> questionID { get; set; }
        public Nullable<int> selectedOptionID { get; set; }
        public Nullable<System.DateTime> createdAt { get; set; }
    
        public virtual Options Options { get; set; }
        public virtual Questions Questions { get; set; }
        public virtual Quizzes Quizzes { get; set; }
        public virtual Users Users { get; set; }
    }
}
