using System.Data.Entity.Core;
using System.Linq;
using System.Web.Mvc;

namespace Issues_Tracker.Controllers
{
    public class BoardController : Controller
    {
        IssueTrackerEntities db = new IssueTrackerEntities();

        [HttpGet]
        public ActionResult Index(string projectName)
        {
            Project project;

            if (!string.IsNullOrEmpty(projectName))
            {
                project = db.Projects.FirstOrDefault(p => p.Name == projectName);
            }
            else
            {
                project = new Project();
            }

            SelectList listOfProjects = new SelectList(db.Projects.Select(p => p.Name));
            try
            {
                listOfProjects.Count();
                ViewBag.Projects = listOfProjects;
                ViewBag.isNull = 1;
            }
            catch (EntityException)
            {
                ViewBag.isNull = 0;
            }

            try
            {
                project.Issues = db.Issues.Where(i => i.Project.Name == project.Name).ToList();
            }
            catch (EntityException)
            {

            }
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
            return RedirectToAction("Index", project.Name);
        }
    }
}