using System;
using System.ComponentModel.DataAnnotations;


namespace Issues_Tracker.Models.Login
{
    public class ChangeDataModel
    {
        public string UserId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime DateOfBirthday { get; set; }

        public string PhoneNumber { get; set; }
    }
}