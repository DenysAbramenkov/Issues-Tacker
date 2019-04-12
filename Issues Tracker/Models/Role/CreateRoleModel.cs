using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Issues_Tracker.Models
{
    public class CreateRoleModel
    {
        [Required(ErrorMessage = "Required field")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Description { get; set; }
    }
}