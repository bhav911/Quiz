using OnlineStoreHelper.Helpers;
using QuizComputation_490_Model.Context;
using QuizComputation_490_Model.CustomModels;
using QuizComputation_490_Repository.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace QuizComputation_490_WebAPI.Controllers
{
    public class LoginAPIController : ApiController
    {
        private readonly AdminService _admin = new AdminService();
        private readonly UserService _user = new UserService();


        [HttpPost]
        [Route("api/LoginAPI/AuthenticateAdmin")]
        public NewRegistration AuthenticateAdmin(LoginModel credential)
        {
            Admins admin = _admin.AuthenticateAdmin(credential);
            if (admin == null)
                return null;
            NewRegistration response = ModelConverter.ConvertAdminToNewUser(admin);
            return response;
        }

        [HttpPost]
        [Route("api/LoginAPI/AuthenticateUser")]
        public NewRegistration AuthenticateUser(LoginModel credential)
        {
            Users user = _user.AuthenticateUser(credential);
            if (user == null)
                return null;
            NewRegistration response = ModelConverter.ConvertUserToNewUser(user);
            return response;
        }

        [HttpPost]
        [Route("api/LoginAPI/SignUp")]
        public bool SignUp(NewRegistration newUser)
        {
            Users user = ModelConverter.ConvertNewUserToUser(newUser);
            string profileData = newUser.profile;
            bool status = _user.RegisterUser(user, profileData);
            return status;
        }
    }
}