using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class User
    {
        public User()
        {
            Posts = new HashSet<Post>();
            PostsBookmarks = new HashSet<PostsBookmark>();
            UsersFollows = new HashSet<UsersFollow>();
            UsersPostsSubs = new HashSet<UsersPostsSub>();
            UsersResources = new HashSet<UsersResource>();
            UsersRoles = new HashSet<UsersRole>();
        }

        public string Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime Created { get; set; }
        public string ImageName { get; set; }
        public DateTime? BirthDate { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<PostsBookmark> PostsBookmarks { get; set; }
        public virtual ICollection<UsersFollow> UsersFollows { get; set; }
        public virtual ICollection<UsersPostsSub> UsersPostsSubs { get; set; }
        public virtual ICollection<UsersResource> UsersResources { get; set; }
        public virtual ICollection<UsersRole> UsersRoles { get; set; }
    }
}
