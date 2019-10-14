using QAP4.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace QAP4.Domain.AggreatesModels.Posts.Models
{
    public class Tag : Entity
    {
        public string Name { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public int CreatorUserId { get; set; }
    }
}
