using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizComputation_490_Repository.Services
{
    public class UserService : IUserInterface
    {
        private readonly QuizComputation_490Entities db = new QuizComputation_490Entities();

        public void RegisterUser(Users newUser)
        {
            db.Users.Add(newUser);
            db.SaveChanges();
        }
        public Users AuthenticateUser(LoginModel credentials)
        {
            Users result = db.Users.Where(u => u.email == credentials.Login_email && u.password == credentials.Login_password).FirstOrDefault();
            return result;
        }

        public Users GetProfile(int userID)
        {
            Users user = db.Users.FirstOrDefault(u => u.userID == userID);
            return user;
        }

        public void updateProfile(NewRegistration updatedInfo, int userID)
        {
            Users user = db.Users.FirstOrDefault(u => u.userID == userID);
            user.username = updatedInfo.Username;
            user.password = updatedInfo.Password;
            user.email = updatedInfo.Email;
            user.updatedAt = System.DateTime.Now;
            db.SaveChanges();
        }

    }
}
