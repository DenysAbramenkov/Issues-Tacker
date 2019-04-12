using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Issues_Tracker.Models;
using System.Data.Entity.Core;
using PagedList.Mvc;
using PagedList;

namespace Issues_Tracker.Controllers
{
    [Authorize(Roles = "Developer, QA, Project Manager")]
    public class IssueController : Controller
    {
        IssueTrackerEntities db = new IssueTrackerEntities();

        [HttpGet] 
        public ActionResult Index(string projectName, string priority, int? page)
        {

            IssuePrioritiesList viewIssueList = new IssuePrioritiesList();
            try
            {
                List<Issue> issues = db.Issues.ToList();
                if (!string.IsNullOrEmpty(projectName))
                {
                    if (projectName != "All")
                    {
                        issues = issues.Where(issue => issue.Project.Name.Contains(projectName)).ToList();
                    }
                }

                if (!string.IsNullOrEmpty(priority))
                {
                    if (priority != "All")
                    {
                        issues = issues.Where(x => x.Priority.Name.Contains(priority)).ToList();
                    }
                }

                int pageSize = 8;
                int pageNumber = (page ?? 1);
                viewIssueList.Issues = issues.ToPagedList(pageNumber, pageSize);

                List<string> Projects = new List<string>(db.Projects.Select(p => p.Name));
                Projects.Add("All");
                ViewBag.Projects = new SelectList(Projects);

                List<string> Priorityes = new List<string>(db.Priorities.Select(p => p.Name));
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
            GetUpdatedOrNewIssue(issue);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult AddEditIssue(int issueId)
        {
            IssueView issueView = new IssueView
            { 
                States = new SelectList(db.States, "Id", "Name"),
                Asignee = new SelectList(db.Users, "Id", "Name"),
                Projects = new SelectList(db.Projects, "Id", "Name"),
                Priorityes = new SelectList(db.Priorities, "Id", "Name")
            };

            if (issueId > 0)
            {
                issueView.Issue = db.Issues.FirstOrDefault((issue => issue.Id == issueId));
            }
            else
            {
                issueView.Issue = new Issue();
            }
           
            return PartialView("AddEditIssue", issueView);
        }

        private void GetUpdatedOrNewIssue(Issue issue)
        {
            Issue issueToUpdate = new Issue();
            if (issue.Id > 0)
            {
                issueToUpdate = db.Issues.FirstOrDefault(x => x.Id == issue.Id);
                issueToUpdate.Number = issue.Number;
                issueToUpdate.PriorityId = issue.PriorityId;
                issueToUpdate.ProjectId = issue.ProjectId;
                issueToUpdate.StateId = issue.StateId;
                issueToUpdate.AssigneeId = issue.AssigneeId;
                issueToUpdate.Descripton = issue.Descripton;
                issueToUpdate.Summary = issue.Summary;
                issueToUpdate.Project = db.Projects.FirstOrDefault(x => x.Id == issue.ProjectId);
                issueToUpdate.State = db.States.FirstOrDefault(x => x.Id == issue.StateId);
                issueToUpdate.Priority = db.Priorities.FirstOrDefault(x => x.Id == issue.PriorityId);
                issueToUpdate.User = db.Users.FirstOrDefault(x => x.Id == issue.AssigneeId);
                db.SaveChanges();
            }
            else
            {
                issueToUpdate = issue;
                issueToUpdate.Number = GetNewNumber();
                issueToUpdate.Project = db.Projects.FirstOrDefault(x => x.Id == issue.ProjectId);
                issueToUpdate.State = db.States.FirstOrDefault(x => x.Id == issue.StateId);
                issueToUpdate.Priority = db.Priorities.FirstOrDefault(x => x.Id == issue.PriorityId);
                issueToUpdate.User = db.Users.FirstOrDefault(x => x.Id == issue.AssigneeId);
                db.Issues.Add(issueToUpdate);
                db.SaveChanges();
            }         
        }

        private int GetNewNumber()
        {
            return db.Issues.Count() + 1;
        }
    }
}