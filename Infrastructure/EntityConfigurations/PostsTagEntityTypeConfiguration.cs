using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QAP4.Domain.AggreatesModels.Posts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Infrastructure.EntityConfigurations
{
    public class PostsTagEntityTypeConfiguration : IEntityTypeConfiguration<PostsTag>
    {
        public void Configure(EntityTypeBuilder<PostsTag> configuration)
        {
            //TODO: PostsTags
            configuration.ToTable("PostsTag");
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.IsDelete);
            configuration.Property(c => c.CreationDate);
            configuration.Property(c => c.TagId);
            configuration.Property(c => c.PostsId);
        }
    }
}
