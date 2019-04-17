using Issues_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace Issues_Tracker.Controllers
{
    public class AccountController : Controller
    {
        private ApplicationUserManager UserManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
        }

        ApplicationContext db = new ApplicationContext();

        [Authorize(Roles = "Admin")]
        public ActionResult GetUserList()
        {
            List<UserView> userList = new List<UserView>();
            try
            {
                foreach (ApplicationUser user in db.Users.ToList())
                {
                    UserView u = new UserView();
                    u.User = user;
                    u.UserRoles = UserManager.GetRoles(user.Id).ToList();
                    userList.Add(u);
                }
            }
            catch (SqlException)
            { }
            return View(userList);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email };
                    IdentityResult result = UserManager.Create(user, model.Password);
                    if (result.Succeeded)
                    {
                        var code = UserManager.GenerateEmailConfirmationToken(user.Id);
                        var callbackUrl = Url.Action("ConfirmEmail", "Account", new { userId = user.Id, code = code },
                                   protocol: Request.Url.Scheme);
                        UserManager.SendEmail(user.Id, "Email confirmation",
                                   "To finish registrarion, folow this link:<a href=\""
                                                           + callbackUrl + "\">Finish Registation</a>");
                        return RedirectToAction("Login", "Account");
                    }
                    else
                    {
                        foreach (string error in result.Errors)
                        {
                            ModelState.AddModelError("", error);
                        }
                    }
                }
                catch(SqlException)
                { }
            }
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId, string code)
        {
            try
            {
                ApplicationUser user = UserManager.FindById(userId);
                if (user != null)
                {
                    if (user.Id == userId)
                    {
                        user.EmailConfirmed = true;
                        UserManager.Update(user);
                        return RedirectToAction("Login", "Account");
                    }
                }
            }
            catch (SqlException)
            { }
            return RedirectToAction("Register", "Account");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    ApplicationUser user = UserManager.Find(model.Email, model.Password);
                    if (user == null)
                    {
                        ModelState.AddModelError("", "Incorect login or password.");
                    }
                    else
                    {
                        ClaimsIdentity claim = UserManager.CreateIdentity(user,
                                                DefaultAuthenticationTypes.ApplicationCookie);
                        AuthenticationManager.SignOut();
                        AuthenticationManager.SignIn(new AuthenticationProperties
                        {
                            IsPersistent = true
                        }, claim);
                        if (String.IsNullOrEmpty(returnUrl))
                            return RedirectToAction("Index", "Issue");
                        return Redirect(returnUrl);
                    }
                }
                catch(SqlException)
                { }
            }
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        private ApplicationRoleManager RoleManager
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ApplicationRoleManager>();
            }
        }

        [Authorize(Roles = "Admin")]
        public ActionResult GetChangeRole(string id)
        {
            try
            {
                ViewBag.Roles = new SelectList(RoleManager.Roles.ToList(), "Name", "Name");
            }
            catch (SqlException)
            { }
            return View("ChangeRole", UserManager.FindById(id));
        }

        [Authorize(Roles = "Admin")]
        public ActionResult ChangeRole(string userName, string roleName)
        {
            try
            {
                ApplicationUser user = UserManager.FindByName(userName);
                var roles = UserManager.GetRoles(user.Id);
                UserManager.RemoveFromRoles(user.Id, roles.ToArray());
                UserManager.AddToRole(user.Id, roleName);
            }
            catch (SqlException)
            { }
            return RedirectToAction("GetUserList");
        }

    }
}