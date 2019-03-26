using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issues_Tracker.Models
{
    public class IssuePrioritiesList
    {
        public List<Issue> Issues;

        public SelectList Priorityes;

        public string IssuePriority { get; set; }

        public string projectName { get; set; }
    }
}