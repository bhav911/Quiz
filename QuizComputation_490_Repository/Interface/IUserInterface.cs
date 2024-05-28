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
        bool RegisterUser(Users newUser);
        Users AuthenticateUser(LoginModel credentials);
        Users GetProfile(int userID);
        bool updateProfile(NewRegistration updatedInfo, int userID);
    }
}
