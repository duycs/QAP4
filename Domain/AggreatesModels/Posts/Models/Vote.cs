using QAP4.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace QAP4.Domain.AggreatesModels.Posts.Models
{
    public class Vote : Entity
    {
        public int PostsId { get; set; }
        /// <summary>
        /// Value: an Id of enum VoteTypes
        /// </summary>
        public int VoteTypeId { get; set; }
        /// <summary>
        /// Value: an Id of user who vote this posts
        /// </summary>
        public int CreatorUserId { get; set; }
        public string CreatorUserName { get; set; }
        /// <summary>
        /// Value: true is up-vote or false is down-vote
        /// </summary>
        public bool IsUpVote { get; set; }
    }
}
