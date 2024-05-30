using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Model.CustomModels
{
    public class NewRegistration
    {
        public string Userrole { get; set; }
        public int UserID { get; set; }

        [Required(ErrorMessage = "Can't Leave Field Empty")]
        [RegularExpression("^[A-Za-z0-9_ ]*$", ErrorMessage = "Only alphanumeric and '-' are allowed")]
        [MaxLength(length: 20, ErrorMessage = "Length must be less than 20 characters")]
        [MinLength(length: 8, ErrorMessage = "Length must be greater than 8 characters")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Can't Leave Field Empty")]
        [MaxLength(length: 20, ErrorMessage = "Length must be less than 20 characters")]
        [MinLength(length: 8, ErrorMessage = "Length must be greater than 8 characters")]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password does not match")]
        public string Confirm_password { get; set; }

        [Required(ErrorMessage = "Can't Leave Field Empty")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }
        public string profile { get; set; }
        public string aadharCard { get; set; }
        public string Marksheet12 { get; set; }
        public string Marksheet10 { get; set; }
        public bool delete_aadhar_card { get; set; }
        public bool delete_Marksheet12 { get; set; }
        public bool delete_Marksheet10 { get; set; }
        public bool delete_profile { get; set; }
        public NewRegistration()
        {
            delete_aadhar_card = false;
            delete_Marksheet12 = false;
            delete_Marksheet10 = false;
            delete_profile = false;
        }
    }
}
