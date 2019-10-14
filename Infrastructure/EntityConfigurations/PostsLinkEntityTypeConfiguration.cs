using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QAP4.Domain.AggreatesModels.Posts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Infrastructure.EntityConfigurations
{
    public class PostsLinkEntityTypeConfiguration : IEntityTypeConfiguration<PostsLink>
    {
        public void Configure(EntityTypeBuilder<PostsLink> configuration)
        {
            //TODO: PostsLinks
            configuration.ToTable("PostLinks");
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.IsDelete);
            configuration.Property(c => c.CreationDate);
            configuration.Property(c => c.PostId);
            configuration.Property(c => c.RelatedPostId);
            configuration.Property(c => c.LinkTypeId);
        }
    }
}
