using System;

namespace OnlySubs.Models
{
    public class Comment
    {
        public string UserImage { get; set; } 
        public string Username { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }

    }
}