using System;

namespace OnlySubs.ViewModels.Responses
{
    public class PostsResponse
    {
        public string PostId { get; set; }
        public string Username { get; set; }
        public string ProfileImage { get; set; }
        public string ImageName { get; set; }
        public DateTime Created { get; set; }
        public int Price { get; set; }
    }
}