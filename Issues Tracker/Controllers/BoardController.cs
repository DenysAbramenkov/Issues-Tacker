using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issues_Tracker.Controllers
{
    public class BoardController : Controller
    {

        IssueTrackerEntities db = new IssueTrackerEntities();

        public ActionResult Index(string projectName)
        {
            Project project;
            if (!string.IsNullOrEmpty(projectName))
            {
                project = db.Projects.FirstOrDefault(p => p.Name == projectName);
            }
            else project = db.Projects.First();
            ViewBag.Projects = new SelectList(db.Projects.Select(p => p.Name));
            project.Issues = db.Issues.Where(i => i.Project.Name == project.Name).ToList();
            return View(project);
        }
    }
}