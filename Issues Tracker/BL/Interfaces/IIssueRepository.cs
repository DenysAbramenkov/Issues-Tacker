using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Issues_Tracker.BL.Interfaces
{
    public interface IIssueRepository: IRepository<Issue>
    {
        IEnumerable<Issue> GetAll(string projectName, string priority);

        void AddOrUpdateIssue(Issue issue);
    }
}
