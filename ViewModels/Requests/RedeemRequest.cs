using System.ComponentModel.DataAnnotations;

namespace OnlySubs.ViewModels.Requests
{
    public class RedeemRequest
    {
        [Required(ErrorMessage = "Enter your code.")]
        [StringLength(16, MinimumLength = 4, ErrorMessage = "Code letters between 4 - 16.")]
        public string code { get; set; }
    }
}