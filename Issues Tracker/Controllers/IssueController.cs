using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Issues_Tracker.Models;

namespace Issues_Tracker.Controllers
{
    public class IssueController : Controller
    {
        IssueTrackerEntities db = new IssueTrackerEntities();

        [HttpGet]
        public ActionResult Index(string projectName, string priority)
        {
            IQueryable<string> priorityQuery = from p in db.Priorities
                                            select p.Name;

            List<Issue> issues = db.Issues.ToList();
            if (!string.IsNullOrEmpty(projectName))
            {
                issues = issues.Where(issue => issue.Project.Name.Contains(projectName)).ToList();
            }

            if (!string.IsNullOrEmpty(priority))
            {
                issues = issues.Where(x => x.Priority.Name.Contains(priority)).ToList();
            }
            IssuePrioritiesList viewIssueList = new IssuePrioritiesList { Issues = issues };

            viewIssueList.Priorityes = new SelectList(priorityQuery.ToList());
            return View(viewIssueList);
        }

        [HttpPost]
        public ActionResult Index(Issue issue)
        {
            if (issue.Id > 0)
            {
                Issue issueToUpdate = db.Issues.FirstOrDefault(x => x.Id == issue.Id) ;
                issueToUpdate.Number = issue.Number;
                issueToUpdate.Priority = db.Priorities.FirstOrDefault(x => x.Name == issue.Priority.Name);
                issueToUpdate.PriorityId = issueToUpdate.Priority.Id;
                issueToUpdate.State = db.States.FirstOrDefault(x => x.Name == issue.State.Name);
                issueToUpdate.StateId = issueToUpdate.State.Id;
                issueToUpdate.Project = db.Projects.FirstOrDefault(x => x.Name == issue.Project.Name);
                issueToUpdate.ProjectId = issueToUpdate.Project.Id;
                issueToUpdate.User = db.Users.FirstOrDefault(x => x.Name == issue.User.Name);
                issueToUpdate.AssigneeId = issueToUpdate.User.Id;
                issueToUpdate.Summary = issue.Summary;
                issueToUpdate.Descripton = issue.Descripton;
                db.SaveChanges();
            }

            else
            {
                Issue issueToCreate = new Issue();
                issueToCreate.Number = issue.Number;
                issueToCreate.Priority = db.Priorities.FirstOrDefault(x => x.Name == issue.Priority.Name);
                issueToCreate.PriorityId = issueToCreate.Priority.Id;
                issueToCreate.State = db.States.FirstOrDefault(x => x.Name == issue.State.Name);
                issueToCreate.StateId = issueToCreate.State.Id;
                issueToCreate.Project = db.Projects.FirstOrDefault(x => x.Name == issue.Project.Name);
                issueToCreate.ProjectId = issueToCreate.Project.Id;
                issueToCreate.User = db.Users.FirstOrDefault(x => x.Name == issue.User.Name);
                issueToCreate.AssigneeId = issueToCreate.User.Id;
                issueToCreate.Summary = issue.Summary;
                issueToCreate.Descripton = issue.Descripton;
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult AddEditIssue(int issueId)
        {
            IssueView issueView = new IssueView
            {
                States = new SelectList(db.States.Select(x => x.Name)),
                Asignee = new SelectList(db.Users.Select(x => x.Name)),
                Projects = new SelectList(db.Projects.Select(x => x.Name)),
                Priorityes = new SelectList(db.Priorities.Select(x => x.Name))
            };

            if (issueId > 0)
            {
                issueView.Issue = db.Issues.FirstOrDefault((issue => issue.Id == issueId));
            }

            return PartialView("AddEditIssue", issueView);
        }
    }
}