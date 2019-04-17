using Issues_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issues_Tracker.Controllers
{
    [Authorize(Roles = "Admin")]
    public class RoleController : Controller
    {
        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        public ActionResult Index()
        {
            return View(RoleManager.Roles.ToList());
        }

        public ActionResult CreateRole()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CreateRole(CreateRoleModel model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    IdentityResult result = RoleManager.Create(new ApplicationRole
                    {
                        Name = model.Name,
                        Description = model.Description
                    });
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Something goes wrong");
                    }
                }
            }
            catch (SqlException)
            { }
            return View(model);
        }

        public ActionResult GetEditRole(string id)
        {

            ApplicationRole role = new ApplicationRole();
            try
            {
                role = RoleManager.FindById(id);
            }
            catch (SqlException)
            { }
            return View("EditRole", role);
        }

        [HttpPost]
        public ActionResult EditRole(ApplicationRole model)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ApplicationRole role = RoleManager.FindById(model.Id.ToString());
                    role.Name = model.Name;
                    role.Description = model.Description;
                    IdentityResult result = RoleManager.Update(RoleManager.FindById(model.Id));
                    if (result.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }
            catch(SqlException)
            { }
            return RedirectToAction("Index");
        }
    }
}