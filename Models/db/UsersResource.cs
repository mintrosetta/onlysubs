using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class UsersResource
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int? Money { get; set; }
        public int? Krama { get; set; }

        public virtual User User { get; set; }
    }
}
