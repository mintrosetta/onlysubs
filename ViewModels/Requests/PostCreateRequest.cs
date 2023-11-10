using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnlySubs.ViewModels.Requests
{
    public class PostCreateRequest
    {
        [Required(ErrorMessage = "Please enter description")]
        [StringLength(256, MinimumLength = 8, ErrorMessage = "Enter letters between 8 - 256.")]
        public string Description { get; set; }
        [Required(ErrorMessage = "Please enter your price")]
        public int Price { get; set; }
        [Required(ErrorMessage = "Please upload your images.")]
        public List<IFormFile> Images { get; set; }
    }
}