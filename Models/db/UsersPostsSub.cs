﻿using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class UsersPostsSub
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string PostId { get; set; }
        public DateTime Created { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
