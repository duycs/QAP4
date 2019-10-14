using QAP4.Domain.Core.Models;
using System;
using System.Collections.Generic;

namespace QAP4.Domain.AggreatesModels.Posts.Models
{
    public class Posts : Entity, IAggregateRoot
    {
        /// <summary>
        /// Existing: when a posts have answer and this answer is accepted as best correct by owner user
        /// </summary>
        public int AcceptedAnswerPostsId { get; set; }
        public int AnswerCount { get; set; }
        /// <summary>
        /// Value: string trip from htmlContent
        /// </summary>
        public string BodyContent { get; set; }
        /// <summary>
        /// Value: JSON of comments, exp: 
        /// </summary>
        public string Comments { get; set; }
        public int CommentCount { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? ClosedDate { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime? CommunityOwnedDate { get; set; }
        /// <summary>
        /// Values: link url or base64
        /// </summary>
        public string CoverImage { get; set; }
        public string Description { get; set; }
        /// <summary>
        /// Value: an Id in enum PostsTypes
        /// </summary>
        public int PostsTypeId { get; set; }
        /// <summary>
        /// Value: an Id of posts in the current table
        /// </summary>
        public int ParentId { get; set; }
        /// <summary>
        /// Value: JSON of childs posts or related posts, 
        /// exp: [{"id": 1, "title": "string", "HeadContent: "string",
        /// "tags": [{"id":1, "name":"string"}],
        /// "ownerUserId": 1, "ownerUserName": "string", "ownerUserAvatar": "string", "publishedDate": "string", "voteCount": 1, "viewCount": 1,
        /// "score": 1, "friendlyUrl": "string"}]
        /// Existing: when the posts have child or related posts
        /// </summary>
        public string RelatedPosts { get; set; }
        /// <summary>
        /// Note: score for evaluate posts by many counting of view, vote and AI
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// Value: raw html content
        /// </summary>
        public string HtmlContent { get; set; }
        public string HeadContent { get; set; }
        /// <summary>
        /// Value: true is published, false is un-published, default is false
        /// </summary>
        public bool IsPublished { get; set; }
        /// <summary>
        /// Value: JSON of tags in posts, exp: [{"id":1, "name":"string"}]
        /// </summary>
        public string Tags { get; set; }
        public string Title { get; set; }
        /// <summary>
        /// Note: this is user who create this posts
        /// </summary>
        public int OwnerUserId { get; set; }
        public string OwnerUserName { get; set; }
        public string OwnerUserAvatar { get; set; }
        /// <summary>
        /// Existing: when user click published the posts
        /// </summary>
        public DateTime? PublishedDate { get; set; }
        /// <summary>
        /// Existing: after saved posts, user edit posts
        /// </summary>
        public DateTime? LastEditedDate { get; set; }
        /// <summary>
        /// Existing: when more user help edit the posts of owner user
        /// </summary>
        public int LastEditorUserId { get; set; }
        /// <summary>
        /// Value: JSON of voters, exp: [{"id": 1, "name": "string"}]
        /// </summary>
        public string Voters { get; set; }
        public int VoteCount { get; set; }
        public int ViewCount { get; set; }
        /// <summary>
        /// Value: friendly url by title and Id, exp: title-name---id
        /// </summary>
        public string FriendlyUrl { get; set; }

    }
}
