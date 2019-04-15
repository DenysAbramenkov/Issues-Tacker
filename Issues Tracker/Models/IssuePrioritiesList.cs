using System.Collections.Generic;
using System.Web.Mvc;
using PagedList;


namespace Issues_Tracker.Models
{
    public class IssuePrioritiesList
    {
        public IPagedList<Issue> Issues { get; set; }

        public SelectList Priorityes { get; set; }

        public string IssuePriority { get; set; }

        public string projectName { get; set; }
    }
}