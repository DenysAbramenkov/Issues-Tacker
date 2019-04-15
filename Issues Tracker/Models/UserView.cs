using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Issues_Tracker.Models
{
    public class UserView
    {
        public ApplicationUser User { get; set; }

        public List<string> UserRoles { get; set; }
    }
}