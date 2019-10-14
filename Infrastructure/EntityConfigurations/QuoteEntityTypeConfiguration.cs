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
    public class QuoteEntityTypeConfiguration : IEntityTypeConfiguration<Quote>
    {
        public void Configure(EntityTypeBuilder<Quote> configuration)
        {
            configuration.ToTable("Quotes");
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.IsDelete);
            configuration.Property(c => c.CreationDate);
            configuration.Property(c => c.Content);
            configuration.Property(c => c.UserId);
            configuration.Property(c => c.Content);
            configuration.Property(c => c.AuthorDisplayName);
        }
    }
}
