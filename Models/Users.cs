using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Users
    {
        public int Id { get; set; }
        public int? AccountId { get; set; }
        public string AccountName{get;set;}
        public string Address { get; set; }
        public int? Age { get; set; }
        public string AboutMe { get; set; }
        public string Avatar { get; set; }
        public string BannerImg { get; set; }
        public DateTime? CreationDate { get; set; }
        public string DisplayName { get; set; }
        public int? DownVotes { get; set; }
        public DateTime? DoB { get; set; }
        public string Email { get; set; }
        public int? Reputation { get; set; }
        public int? Rank { get; set; }
        public string Location { get; set; }
        public DateTime? LastAccessDate { get; set; }
        public string LastName { get; set; }
        public string ProfileImageUrl { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public int? PostsCount { get; set; }
        public string FirstName { get; set; }
        public int? FollowedCount { get; set; }
        public int? UpVotes { get; set; }
        public string Status { get; set; }
        public int? Views { get; set; }
        public string WebsiteUrl { get; set; }
    }
}
