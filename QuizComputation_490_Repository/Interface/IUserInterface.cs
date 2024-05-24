using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Repository.Interface
{
    public interface IUserInterface
    {
        void RegisterUser(Users newUser);
        Users AuthenticateUser(LoginModel credentials);
    }
}
