using System.ComponentModel.DataAnnotations;
namespace OnlySubs.ViewModels.Requests
{
    public class UserRegisterRequest
    {
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Enter letters between 6 - 100.", MinimumLength = 6)]
        public string Username { get; set; }
        [Required(ErrorMessage = "Password is required.")]
        [StringLength(24, ErrorMessage = "Enter letters between 8 - 100.", MinimumLength = 8)]
        [RegularExpression("^[A-Za-z0-9_@-]+$", ErrorMessage = "Password should be letters, lowercase letters, uppercase letters and numbers.")]
        public string Password { get; set; }
        [Required(ErrorMessage = "Confirm Password is required.")]
        [StringLength(24, ErrorMessage = "Enter letters between 8 - 100.", MinimumLength = 8)]
        [RegularExpression("^[A-Za-z0-9_@-]+$", ErrorMessage = "Confirm password should be letters, lowercase letters, uppercase letters and numbers.")]
        [Compare(nameof(Password), ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }
}