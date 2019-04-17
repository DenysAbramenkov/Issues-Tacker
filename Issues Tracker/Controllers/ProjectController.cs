using Issues_Tracker.BL;
using System.Data.Entity.Core;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web.Mvc;

namespace Issues_Tracker.Controllers
{
    [Authorize(Roles = "Project Manager")]
    public class ProjectController : Controller
    {
        IUnitOfWork _context;

        public ProjectController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpPost]
        public ActionResult CreateProject(string projectName)
        {
            try
            {
                if (!_context.Projects.Select(p => p.Name).Contains(projectName))
                {
                    Project project = new Project();
                    project.Name = projectName;
                    _context.Projects.Create(project);
                    _context.Save();
                    ViewBag.failedMassage = string.Empty;
                }
                else
                {
                    ViewBag.failedMassage = "Project with such name already exist";
                }
            }
            catch (DbEntityValidationException)
            {

            }
            catch (EntityException)
            {

            }

            return RedirectToAction("Index", "Issue");
        }
    }
}