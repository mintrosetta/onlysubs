using System;

namespace OnlySubs.Models
{
    public class ProfilePostImage
    {
        public string ImageName { get; set; }
        public string PostId { get; set; }
        public DateTime Created { get; set; }
        public int Price { get; set; }
    }
}