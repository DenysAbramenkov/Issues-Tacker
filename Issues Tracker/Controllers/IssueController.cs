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
    }
}