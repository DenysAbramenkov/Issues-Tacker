using System.ComponentModel.DataAnnotations;

namespace Issues_Tracker.Models
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "Required field")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Required field")]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required(ErrorMessage = "Required field")]
        [Compare("Password", ErrorMessage = "Password not equal")]
        [DataType(DataType.Password)]
        public string PasswordConfirm { get; set; }
    }
}