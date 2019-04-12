using System.ComponentModel.DataAnnotations;

namespace Issues_Tracker.Models
{
    public class EditRoleModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Required field")]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}