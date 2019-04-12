using System.ComponentModel.DataAnnotations;

namespace Issues_Tracker.Models.Login
{
    public class ChangePasswordModel
    {
        public string UserId { get; set; }

        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Compare("Password", ErrorMessage = "Password not equal")]
        [DataType(DataType.Password)]
        public string ConfirmationPassword { get; set; }

        [DataType(DataType.Password)]
        public string NewPassword { get; set; }
    }
}