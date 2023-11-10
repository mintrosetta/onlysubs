using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class Post
    {
        public Post()
        {
            PostsBookmarks = new HashSet<PostsBookmark>();
            PostsComments = new HashSet<PostsComment>();
            PostsImages = new HashSet<PostsImage>();
            PostsLikes = new HashSet<PostsLike>();
            UsersPostsSubs = new HashSet<UsersPostsSub>();
        }

        public string Id { get; set; }
        public string UserId { get; set; }
        public DateTime Created { get; set; }
        public string Description { get; set; }
        public bool IsSub { get; set; }

        public virtual User User { get; set; }
        public virtual PostsPrice PostsPrice { get; set; }
        public virtual ICollection<PostsBookmark> PostsBookmarks { get; set; }
        public virtual ICollection<PostsComment> PostsComments { get; set; }
        public virtual ICollection<PostsImage> PostsImages { get; set; }
        public virtual ICollection<PostsLike> PostsLikes { get; set; }
        public virtual ICollection<UsersPostsSub> UsersPostsSubs { get; set; }
    }
}
