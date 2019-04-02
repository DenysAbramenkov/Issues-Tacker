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

        [HttpPost]
        public ActionResult UpdateState(int issueId, int stateId)
        {
            Issue issue = db.Issues.FirstOrDefault(i => i.Id == issueId);
            issue.StateId = stateId;
            issue.State = db.States.FirstOrDefault(s => s.Id == stateId);
            Project project = db.Projects.FirstOrDefault(p => p.Id == issue.ProjectId);
            db.SaveChanges();
            return View("Index", project.Name);
        }
    }
}