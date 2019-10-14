using QAP4.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace QAP4.Domain.AggreatesModels.Posts.Models
{
    public class PostsTag : Entity
    {
        public int TagId { get; set; }
        public int PostsId { get; set; }
    }
}
