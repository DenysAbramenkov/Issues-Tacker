using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issues_Tracker.Models
{
    public class IssueView
    {
        public Issue Issue;

        public SelectList Priorityes;

        public SelectList Projects;

        public SelectList States;

        public SelectList Asignee;
    }
}