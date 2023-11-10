using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace OnlySubs.ViewModels.Requests
{
    public class UserUpdateRequest
    {
        public IFormFile ImageProfile { get; set; }
        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Enter letters between 6 - 100.", MinimumLength = 6)]
        public string Username { get; set; }
        [DataType(DataType.DateTime)]
        public DateTime? BirthDate { get; set; }
        [StringLength(256, ErrorMessage = "Enter letters between 6 - 256.", MinimumLength = 6)]
        public string Description { get; set; }
    }
}