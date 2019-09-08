using System;
using System.Collections.Generic;

namespace QAP4.Models
{
    public partial class Posts
    {
        public int Id { get; set; }
        public int? AcceptedAnswerId { get; set; }
        public int? AnswerCount { get; set; }
        public string BodyContent { get; set; }
        public DateTime? CreationDate { get; set; }
        public string Comments { get; set; }
        public int? CommentCount { get; set; }
        public DateTime? CloseDate { get; set; }
        public DateTime? CommunityOwnedDate { get; set; }
        public string CoverImg { get; set; }
        public string Description { get; set; }
        public DateTime? DeletionDate { get; set; }
        public int? OwnerUserId { get; set; }
        public byte? PostTypeId { get; set; }
        public int? ParentId { get; set; }
        public string RelatedPosts { get; set; }
        public int? Score { get; set; }
        public string HtmlContent { get; set; }
        public string HeadContent { get; set; }
        public string Tags { get; set; }
        public string Title { get; set; }
        public string TableContent { get; set; }
        public string UserDisplayName { get; set; }
        public string UserAvatar { get; set; }
        public DateTime? LastActivityDate { get; set; }
        public DateTime? LastEditDate { get; set; }
        public int? LastEditorUserId { get; set; }
        public string FriendlyUrl {get;set;}
        public int? VoteCount { get; set; }
        public int? ViewCount { get; set; }
        public string Answer2 { get; set; }
        public string Answer3 { get; set; }
        public string Answer4 { get; set; }
        public string Answer5 { get; set; }
    }
}
