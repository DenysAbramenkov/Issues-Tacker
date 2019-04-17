using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Issues_Tracker.Models;
using System.Data.Entity.Core;
using PagedList;
using Issues_Tracker.BL;

namespace Issues_Tracker.Controllers
{
    [Authorize(Roles = "Developer, QA, Project Manager")]
    public class IssueController : Controller
    {
        IUnitOfWork _context;

        public IssueController(IUnitOfWork context)
        {
            _context = context;
        }

        [HttpGet] 
        public ActionResult Index(string projectName, string priority, int? page)
        {

            IssuePrioritiesList viewIssueList = new IssuePrioritiesList();
            try
            {
                List<Issue> issues = _context.Issues.GetAll(projectName, priority).ToList();
                
                int pageSize = 8;
                int pageNumber = (page ?? 1);
                viewIssueList.Issues = issues.ToPagedList(pageNumber, pageSize);
           
                List<string> Projects = new List<string>(_context.Projects.Select(p => p.Name));
                Projects.Add("All");
                ViewBag.Projects = new SelectList(Projects);

                List<string> Priorityes = new List<string>(_context.Priorityes.Select(p => p.Name));
                Priorityes.Add("All");
                viewIssueList.Priorityes = new SelectList(Priorityes);
            }
            catch (EntityException)
            {

            }

            return View(viewIssueList);  
        }

        [HttpPost]
        public ActionResult Index(Issue issue)
        {
            _context.Issues.AddOrUpdateIssue(issue);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddEditIssue(int issueId)
        {
            IssueView issueView = new IssueView
            { 
                States = new SelectList(_context.States.GetAll(), "Id", "Name"),
                Asignee = new SelectList(_context.Users.GetAll(), "Id", "Name"),
                Projects = new SelectList(_context.Projects.GetAll(), "Id", "Name"),
                Priorityes = new SelectList(_context.Priorityes.GetAll(), "Id", "Name")
            };

            if (issueId > 0)
            {
                issueView.Issue = _context.Issues.Get(issueId);
            }
            else
            {
                issueView.Issue = new Issue();
            }
           
            return PartialView("AddEditIssue", issueView);
        }
    }
}