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
                Issue issueToCreate = new Issue();
                issueToCreate.Number = issue.Number;
                issueToCreate.PriorityId = issue.PriorityId;
                issueToCreate.ProjectId = issue.ProjectId;
                issueToCreate.StateId = issue.StateId;
                issueToCreate.AssigneeId = issue.AssigneeId;
                issueToCreate.Summary = issue.Summary;
                issueToCreate.Descripton = issue.Descripton;
                issueToCreate.Project = db.Projects.FirstOrDefault(x => x.Id == issue.ProjectId);
                issueToCreate.State = db.States.FirstOrDefault(x => x.Id == issue.StateId);
                issueToCreate.Priority = db.Priorities.FirstOrDefault(x => x.Id == issue.PriorityId);
                issueToCreate.User = db.Users.FirstOrDefault(x => x.Id == issue.AssigneeId);
                db.Issues.Add(issueToCreate);
                db.SaveChanges();

            }
            return RedirectToAction("Index");
        }

        public ActionResult AddEditIssue(int issueId)
        {
            IssueView issueView = new IssueView
            { 
                States = new SelectList(db.States, "Id", "Name"),
                Asignee = new SelectList(db.Users, "Id", "Name"),
                Projects = new SelectList(db.Projects, "Id", "Name"),
                Priorityes = new SelectList(db.Priorities, "Id", "Name")
            };

            ViewBag.States = new SelectList(db.States, "Id", "Name");
            ViewBag.Asignee = db.Users.Select(x => x.Name);
            ViewBag.Projects = db.Projects.Select(x => x.Name);
            ViewBag.Priorityes = db.Priorities.Select(x => x.Name);

            if (issueId > 0)
            {
                issueView.Issue = db.Issues.FirstOrDefault((issue => issue.Id == issueId));
            }

            return PartialView("AddEditIssue", issueView);
        }
    }
}