using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;
using OnlySubs.Models;

namespace OnlySubs.ViewModels.Responses
{
    public class PostResponse
    {
        public string PostId { get; set; }
        public string UserImage { get; set; } 
        public string Username { get; set; }
        //public bool IsBookMark { get; set; }
        public int Price { get; set; }
        public List<string> Images { get; set; }
        public int LikesCount { get; set; }
        public string Description { get; set; }
        public List<Comment> Comment { get; set; }
        public DateTime Created { get; set; }
    }
}