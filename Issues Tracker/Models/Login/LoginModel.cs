using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Issues_Tracker.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Required field")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}