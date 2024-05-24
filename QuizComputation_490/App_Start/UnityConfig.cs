using QuizComputation_490_Repository.Interface;
using QuizComputation_490_Repository.Services;
using System.Web.Mvc;
using Unity;
using Unity.Mvc5;

namespace QuizComputation_490
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers
            
            // e.g. container.RegisterType<ITestService, TestService>();
            
            DependencyResolver.SetResolver(new UnityDependencyResolver(container));
            container.RegisterType<IAdminInterface, AdminService>();
            container.RegisterType<IUserInterface, UserService>();
        }
    }
}