using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class Role
    {
        public Role()
        {
            UsersRoles = new HashSet<UsersRole>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime Created { get; set; }

        public virtual ICollection<UsersRole> UsersRoles { get; set; }
    }
}
