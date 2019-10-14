using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QAP4.Domain.AggreatesModels.Posts.Models;

namespace QAP4.Infrastructure.EntityConfigurations
{
    public class VoteEntityTypeConfiguration : IEntityTypeConfiguration<Vote>
    {
        public void Configure(EntityTypeBuilder<Vote> configuration)
        {
            configuration.ToTable("Votes");
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.IsDelete);
            configuration.Property(c => c.PostsId);
            configuration.Property(c => c.VoteTypeId);
            configuration.Property(c => c.UserId);
            configuration.Property(c => c.CreationDate);
            configuration.Property(c => c.PostsId);
            configuration.Property(c => c.BountyAmount);
            configuration.Property(c => c.IsOn);
            configuration.Property(c => c.UserId);
        }
    }
}
