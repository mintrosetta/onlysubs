using System.ComponentModel.DataAnnotations;

namespace OnlySubs.ViewModels.Requests
{
    public class CommentRequest
    {
        [Required(ErrorMessage = "Please enter your description.")]
        [StringLength(256, ErrorMessage = "Enter letters between 8 - 256.", MinimumLength = 8)]
        public string Description { get; set; }
    }
}