using System;
using System.Collections.Generic;

#nullable disable

namespace OnlySubs.Models.db
{
    public partial class RefreshToken
    {
        public int Id { get; set; }
        public string RefreshToken1 { get; set; }
        public string UserId { get; set; }
    }
}
