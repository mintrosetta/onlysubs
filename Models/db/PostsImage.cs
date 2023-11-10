using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class PostsImage
    {
        public int Id { get; set; }
        public string PostId { get; set; }
        public DateTime Created { get; set; }
        public string ImageName { get; set; }

        public virtual Post Post { get; set; }
    }
}
