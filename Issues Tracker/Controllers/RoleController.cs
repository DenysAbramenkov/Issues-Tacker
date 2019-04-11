using Issues_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System;
using System.Collections.Generic;
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
            return View(model);
        }

        [HttpPost]
        public ActionResult GetEditRole(string id)
        {
            ApplicationRole role = RoleManager.FindById(id);
            return View("EditRole", role);
        }

        [HttpPost]
        public ActionResult EditRole(ApplicationRole model)
        {
            if (ModelState.IsValid)
            {
                ApplicationRole role = RoleManager.FindById(model.Id.ToString());
                role.Description = model.Description;
                IdentityResult result = RoleManager.Update(RoleManager.FindById(model.Id.ToString()));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    ModelState.AddModelError("", "Something goes wrong");
                }
            }
            return View(model);
        }
    }
}