using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QAP4.Domain.AggreatesModels.Posts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Infrastructure.EntityConfigurations
{
    public class CommentEntityTypeConfiguration : IEntityTypeConfiguration<Comment>
    {
        public void Configure(EntityTypeBuilder<Comment> configuration)
        {
            configuration.ToTable("Comments");
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.IsDelete);
            configuration.Property(c => c.CreationDate);
            configuration.Property(c => c.CreationByAdmin);
            configuration.Property(c => c.CreationByCurrentUser);
            configuration.Property(c => c.Content);
            configuration.Property(c => c.PostsId);
            configuration.Property(c => c.ParentId);
            configuration.Property(c => c.ProfilePictureUrl);
            configuration.Property(c => c.UserId);
            configuration.Property(c => c.UserDisplayName);
            configuration.Property(c => c.UpvoteCount);
            configuration.Property(c => c.UserHasUpvote);
            configuration.Property(c => c.ModificationDate);
        }
    }
}
