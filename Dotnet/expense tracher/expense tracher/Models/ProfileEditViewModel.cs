using System.ComponentModel.DataAnnotations;

namespace expense_tracher.Models
{
    public class ProfileEditViewModel
    {

        [Required(ErrorMessage = "Username is required.")]
        public string UserName { get; set; }
        [RegularExpression(@"^[6-9]\d{9}$", ErrorMessage = "Invalid phone format.")]
        public string? Phone { get; set; }
        [RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$", ErrorMessage = "Invalid email format.")]
        public string? Email { get; set; }
    }
}
