using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class PostsPrice
    {
        public string PostId { get; set; }
        public int Price { get; set; }
        public DateTime Created { get; set; }

        public virtual Post Post { get; set; }
    }
}
