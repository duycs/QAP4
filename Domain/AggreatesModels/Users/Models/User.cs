using QAP4.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace QAP4.Domain.AggreatesModels.Users.Models
{
    public class User : Entity, IAggregateRoot
    {
        public int AccountId { get; set; }
        /// <summary>
        /// Value: generate by emailName or firstName.lastName---Id
        /// </summary>
        public string AccountName { get; set; }
        public string Address { get; set; }
        public int Age { get; set; }
        public string AboutMe { get; set; }
        /// <summary>
        /// Value: image link or base64
        /// </summary>
        public string AvatarImage { get; set; }
        public string BannerImage { get; set; }
        public string DisplayName { get; set; }
        /// <summary>
        /// Value: an Id of enum VoteTypes
        /// </summary>
        public int Vote { get; set; }
        public DateTime? DoB { get; set; }
        public string Email { get; set; }
        public int Reputation { get; set; }
        /// <summary>
        /// Value: an Id of enum RankTypes
        /// </summary>
        public int Rank { get; set; }
        public string Location { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public string LastAccessUrl { get; set; }

        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int PostsCount { get; set; }
        public string FirstName { get; set; }
        public int FollowedCount { get; set; }
        public int VoteCount { get; set; }
        public string Status { get; set; }
        public string WebsiteUrl { get; set; }
    }
}
