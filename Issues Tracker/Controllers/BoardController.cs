using Issues_Tracker.BL;
using System.Data.Entity.Core;
using System.Linq;
using System.Web.Mvc;

namespace Issues_Tracker.Controllers
{
    [Authorize(Roles = "Developer, QA, Project Manager, Admin")]
    public class BoardController : Controller
    {
        IUnitOfWork _context;

        public BoardController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult Index(string projectName)
        {
            Project project;

            if (!string.IsNullOrEmpty(projectName))
            {
                project = _context.Projects.FirstOrDefault(p => p.Name == projectName);
            }
            else
            {
                project = new Project();
            }

            SelectList listOfProjects = new SelectList(_context.Projects.Select(p => p.Name));

            try
            {
                ViewBag.Projects = listOfProjects;
                ViewBag.isNull = 1;
            }
            catch (EntityException)
            {
                ViewBag.isNull = 0;
            }

            try
            {
                project.Issues = _context.Issues.Find(i => i.Project.Name == project.Name).ToList();
            }
            catch (EntityException)
            {

            }

            return View(project);
        }

        [HttpPost]
        public ActionResult UpdateState(int issueId, int stateId)
        {
            Issue issue = _context.Issues.Get(issueId);
            issue.StateId = stateId;
            Project project = _context.Projects.FirstOrDefault(p => p.Id == issue.ProjectId);
            _context.Save();
            return RedirectToAction("Index", project.Name);
        }
    }
}