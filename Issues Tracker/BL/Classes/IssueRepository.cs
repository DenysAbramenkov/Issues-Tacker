using Issues_Tracker.BL.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;


namespace Issues_Tracker.BL.Classes
{
    public class IssueRepository : Repository<Issue>, IIssueRepository
    {
        public IssueRepository(IssueTrackerEntities context) :base(context)
        {

        }

        public IEnumerable<Issue> GetAll(string projectName, string priority)
        {
            List<Issue> issues = GetAll().ToList();
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
            return issues;
        }

        public void AddOrUpdateIssue(Issue issue)
        {
            Issue issueToUpdate = new Issue();
            if (issue.Id > 0)
            {
                issueToUpdate = Get(issue.Id);
                issueToUpdate.Number = issue.Number;
                issueToUpdate.PriorityId = issue.PriorityId;
                issueToUpdate.ProjectId = issue.ProjectId;
                issueToUpdate.StateId = issue.StateId;
                issueToUpdate.AssigneeId = issue.AssigneeId;
                issueToUpdate.Descripton = issue.Descripton;
                issueToUpdate.Summary = issue.Summary;
                issueToUpdate.Project = Context.Set<Project>().FirstOrDefault(x => x.Id == issue.ProjectId);
                issueToUpdate.State = Context.Set<State>().FirstOrDefault(x => x.Id == issue.StateId);
                issueToUpdate.Priority = Context.Set<Priority>().FirstOrDefault(x => x.Id == issue.PriorityId);
                issueToUpdate.User = Context.Set<User>().FirstOrDefault(x => x.Id == issue.AssigneeId);
            }
            else
            {
                issueToUpdate = issue;
                issueToUpdate.Number = GetNewNumber();
                issueToUpdate.Project = Context.Set<Project>().FirstOrDefault(x => x.Id == issue.ProjectId);
                issueToUpdate.State = Context.Set<State>().FirstOrDefault(x => x.Id == issue.StateId);
                issueToUpdate.Priority = Context.Set<Priority>().FirstOrDefault(x => x.Id == issue.PriorityId);
                issueToUpdate.User = Context.Set<User>().FirstOrDefault(x => x.Id == issue.AssigneeId);
                Create(issueToUpdate);
            }
                Context.SaveChanges();
        }
           

        private int GetNewNumber()
        {
            return Context.Set<Issue>().Count() + 1;
        }
    }
}