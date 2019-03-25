using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issues_Tracker.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            using (IssueTrackerEntities db = new IssueTrackerEntities())
            {
                foreach (Priority p in db.Priorities)
                {
                    ViewBag.Message = "Your priority is " + p.Name;
                }                
            }
            return View();
        }

        public ActionResult Contact()
        {
           

            return View();
        }
    }
}