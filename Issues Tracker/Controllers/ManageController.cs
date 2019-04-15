using Issues_Tracker.Models;
using Issues_Tracker.Models.Login;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
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
            model.UserId = UserManager.FindByName(User.Identity.Name).Id; 
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangePassword(ChangePasswordModel model)
        {
            ApplicationUser user = UserManager.Find(UserManager.FindById(model.UserId).UserName, model.Password);
            if (user != null)
            {
                UserManager.RemovePassword(user.Id);
                UserManager.AddPassword(user.Id, model.NewPassword);
            }
            return View("Index");
        }

        public ActionResult ChangeUserData()
        {
            ChangeDataModel model = new ChangeDataModel();
            model.UserId = UserManager.FindByName(User.Identity.Name).Id;
            return View(model);
        }

        [HttpPost]
        public ActionResult ChangeUserData(ChangeDataModel model)
        {
            ApplicationContext db = new ApplicationContext();
            ApplicationUser user = UserManager.FindById(model.UserId);
            if (user != null)
            {
                user.PhoneNumber = model.PhoneNumber;
                user.FirstName = model.FirstName;
                user.LastName = model.LastName;
                user.DateOfBirthday = model.DateOfBirthday;
                db.SaveChanges();
            }
            return View("Index");
        }
    }
}