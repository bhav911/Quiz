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

        public bool RegisterUser(Users newUser, string profileData)
        {
            try
            {
                Users user = db.Users.Add(newUser);
                db.SaveChanges();
                UserProfile userProfile = new UserProfile()
                {
                    profileContent = profileData,
                    userID = user.userID
                };
                db.UserProfile.Add(userProfile);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }
        public Users AuthenticateUser(LoginModel credentials)
        {
            try
            {
                Users result = db.Users.Where(u => u.email == credentials.Login_email && u.password == credentials.Login_password).FirstOrDefault();
                return result;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public Users GetProfile(int userID)
        {
            try
            {
                Users user = db.Users.FirstOrDefault(u => u.userID == userID);
                return user;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public bool updateProfile(NewRegistration updatedInfo, int userID)
        {
            try
            {
                Users user = db.Users.FirstOrDefault(u => u.userID == userID);
                user.username = updatedInfo.Username;
                user.password = updatedInfo.Password;
                user.email = updatedInfo.Email;
                user.updatedAt = System.DateTime.Now;
                if (updatedInfo.profile != null)
                    user.UserProfile.FirstOrDefault(u => u.userID == userID).profileContent = updatedInfo.profile;
                if (updatedInfo.aadharCard != null || updatedInfo.delete_aadhar_card)
                    user.UserProfile.FirstOrDefault(u => u.userID == userID).aadharCard = updatedInfo.aadharCard;
                if (updatedInfo.Marksheet12 != null || updatedInfo.delete_Marksheet12)
                    user.UserProfile.FirstOrDefault(u => u.userID == userID).Marksheet12 = updatedInfo.Marksheet12;
                if (updatedInfo.Marksheet10 != null || updatedInfo.delete_Marksheet10)
                    user.UserProfile.FirstOrDefault(u => u.userID == userID).Marksheet10 = updatedInfo.Marksheet10;
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public void GetMultiMedia(NewRegistration user)
        {
            user.aadharCard = db.UserProfile.FirstOrDefault(u => u.userID == user.UserID).aadharCard;
            user.profile = db.UserProfile.FirstOrDefault(u => u.userID == user.UserID).profileContent;
            user.Marksheet12 = db.UserProfile.FirstOrDefault(u => u.userID == user.UserID).Marksheet12;
            user.Marksheet10 = db.UserProfile.FirstOrDefault(u => u.userID == user.UserID).Marksheet10;
        }

    }
}
