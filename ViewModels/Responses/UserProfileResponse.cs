using System.Collections.Generic;
using OnlySubs.Models;

namespace OnlySubs.ViewModels.Responses
{
    public class UserProfileResponse
    {
        public string ImageName {  get; set; }
        public string Username { get; set; }
        public int? Krama { get; set; }
        public int Follower {  get; set; }
        public int Following {  get; set; }
        public string Description {  get; set; }
        public List<ProfilePostImage> PostsImage {  get; set; }
    }
}