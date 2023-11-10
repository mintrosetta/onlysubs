using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class CodeResource
    {
        public string Id { get; set; }
        public DateTime Created { get; set; }
        public int Resource { get; set; }
    }
}
