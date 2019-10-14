using QAP4.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace QAP4.Domain.AggreatesModels.Posts.Models
{
    public class Quote : Entity
    {
        public string Content { get; set; }
        public int CreatorUserId { get; set; }
        public int CreatorUserName { get; set; }
        /// <summary>
        /// Note: real author of this quote, may be create to become an user
        /// </summary>
        public string AuthorName { get; set; }
    }
}
