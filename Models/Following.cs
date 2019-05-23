using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Following
    {
        public int FollowingUserId { get; set; }
        public int FollowedUserId { get; set; }
    }
}
