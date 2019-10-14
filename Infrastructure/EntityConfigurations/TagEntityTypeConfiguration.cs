using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QAP4.Domain.AggreatesModels.Posts;
using QAP4.Domain.AggreatesModels.Posts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QAP4.Infrastructure.EntityConfigurations
{
    public class TagEntityTypeConfiguration : IEntityTypeConfiguration<Tag>
    {
        public void Configure(EntityTypeBuilder<Tag> configuration)
        {
            configuration.ToTable("Tags");
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.IsDelete);
            configuration.Property(c => c.CreationDate);
            configuration.Property(c => c.Name);
            configuration.Property(c => c.Count);
            configuration.Property(c => c.Description);
            configuration.Property(c => c.ExcerptPostId);
            configuration.Property(c => c.WikiPostId);
            configuration.Property(c => c.UserCreatedId);
        }
    }
}
