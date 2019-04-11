using Issues_Tracker.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
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

        public ActionResult GetUserList()
        {
            List<UserView> userList = new List<UserView>();
            foreach (ApplicationUser user in db.Users.ToList())
            {
                UserView u = new UserView();
                u.User = user;
                u.UserRoles = UserManager.GetRoles(user.Id).ToList();
                userList.Add(u);
            }
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
                ApplicationUser user = new ApplicationUser { UserName = model.Email, Email = model.Email};
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
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult ConfirmEmail(string userId, string code)
        {
            ApplicationUser user = this.UserManager.FindById(userId);
            if (user != null)
            {
                if (user.Id == userId)
                {
                    user.EmailConfirmed = true;
                    UserManager.Update(user);
                    return RedirectToAction("Login", "Account");
                }
            }
            return RedirectToAction("Register", "Account");
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login(string returnUrl)
        {
            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
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
            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [HttpPost]
        public ActionResult Logout()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Login");
        }

        [HttpPost]
        public ActionResult ChangeRole(string id)
        { 
            return PartialView(UserManager.FindById(id));
        } 


    }
}