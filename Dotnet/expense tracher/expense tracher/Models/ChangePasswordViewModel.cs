 using System.ComponentModel.DataAnnotations;

namespace expense_tracher.Models
{
    public class ChangePasswordViewModel
    {
        [Required(ErrorMessage = "oldPassword is required.")]
        [DataType(DataType.Password)]
        public string OldPassword { get; set; }
        [Required(ErrorMessage = "newPassword is required.")]
        [DataType(DataType.Password)]
        public string NewPassword { get; set; }

        [Required(ErrorMessage = "confirmPassword is required.")]
        [DataType(DataType.Password)]
        public string ConfirmPassword { get; set; }
    }
}
