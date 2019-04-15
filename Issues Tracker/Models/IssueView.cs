using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Issues_Tracker.Models
{
    public class IssueView
    {
        public Issue Issue { get; set; }

        public SelectList Priorityes { get; set; }

        public SelectList Projects { get; set; }

        public SelectList States { get; set; }

        public SelectList Asignee { get; set; }
    }
}