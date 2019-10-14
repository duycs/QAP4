using QAP4.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace QAP4.Domain.AggreatesModels.Posts.Models
{
    public class Comment : Entity
    {
        public bool? CreationByAdmin { get; set; }
        public bool? CreationByCurrentUser { get; set; }
        public string Content { get; set; }
        public int? PostsId { get; set; }
        public int? ParentId { get; set; }
        public string ProfilePictureUrl { get; set; }
        public int? UserId { get; set; }
        public string UserDisplayName { get; set; }
        public int? UpvoteCount { get; set; }
        public bool? UserHasUpvote { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
