using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QAP4.Domain.AggreatesModels.Posts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Infrastructure.EntityConfigurations
{
    public class PostsEntityTypeConfiguration : IEntityTypeConfiguration<Posts>
    {
        public void Configure(EntityTypeBuilder<Posts> configuration)
        {
            configuration.ToTable("Posts");
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.IsDeleted);
            configuration.Property(c => c.CreatedDate);
            configuration.Property(c => c.UpdatedDate);
            configuration.Property(c => c.AcceptedAnswerPostsId);
            configuration.Property(c => c.AnswerCount);
            configuration.Property(c => c.BodyContent);
            configuration.Property(c => c.Comments);
            configuration.Property(c => c.CommentCount);
            configuration.Property(c => c.ClosedDate);
            configuration.Property(c => c.CommunityOwnedDate);
            configuration.Property(c => c.CoverImage);
            configuration.Property(c => c.Description);
            configuration.Property(c => c.DeletedDate);
            configuration.Property(c => c.OwnerUserId);
            configuration.Property(c => c.PostsTypeId);
            configuration.Property(c => c.ParentId);
            configuration.Property(c => c.RelatedPosts);
            configuration.Property(c => c.Score);
            configuration.Property(c => c.HtmlContent);
            configuration.Property(c => c.HeadContent);
            configuration.Property(c => c.Tags);
            configuration.Property(c => c.Title);
            configuration.Property(c => c.TableOfContent);
            configuration.Property(c => c.UserDisplayName);
            configuration.Property(c => c.UserAvatar);
            configuration.Property(c => c.LastActivityDate);
            configuration.Property(c => c.LastEditDate);
            configuration.Property(c => c.LastEditorUserId);
            configuration.Property(c => c.VoteCount);
            configuration.Property(c => c.ViewCount);
        }
    }
}
