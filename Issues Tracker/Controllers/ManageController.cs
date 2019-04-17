using Issues_Tracker.Models;
using Issues_Tracker.Models.Login;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.SqlClient;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;

namespace Issues_Tracker.Controllers
{
    public class ManageController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ChangePassword()
        {
            ChangePasswordModel model = new ChangePasswordModel();
            try
            {
                model.UserId = UserManager.FindByName(User.Identity.Name).Id;
            }
            catch(SqlException)
            { }
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            ApplicationUser user = new ApplicationUser();
            try
            {
                user = UserManager.Find(UserManager.FindById(model.UserId).UserName, model.Password);
                if (user != null)
                {
                    UserManager.RemovePassword(user.Id);
                    UserManager.AddPassword(user.Id, model.NewPassword);
                }
            }
            catch (SqlException)
            { }
            return View("Index");
        }

        public ActionResult ChangeUserData()
        {
            ChangeDataModel model = new ChangeDataModel();
            try
            {
                ApplicationUser user = UserManager.FindByName(User.Identity.Name);
                model.UserId = user.Id;
                model.FirstName = user.FirstName;
                model.PhoneNumber = user.PhoneNumber;
                model.DateOfBirthday = user.DateOfBirthday;
                model.LastName = user.LastName;
            }
            catch (SqlException)
            { }
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeUserData(ChangeDataModel model)
        {
            try
            {
                ApplicationContext db = new ApplicationContext();
                ApplicationUser user = UserManager.FindById(model.UserId);
                if (user != null)
                {
                    user.PhoneNumber = model.PhoneNumber;
                    user.FirstName = model.FirstName;
                    user.LastName = model.LastName;
                    user.DateOfBirthday = model.DateOfBirthday;
                    UserManager.Update(user);
                }
            }
            catch (SqlException)
            { }
            return View("Index");
        }
    }
}