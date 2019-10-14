using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.AggreatesModels.Users.Models;

namespace QAP4.Infrastructure.EntityConfigurations
{
    public class UserEntityTypeConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> configuration)
        {
            configuration.ToTable("Users");
            configuration.HasKey(c => c.Id);
            configuration.Property(c => c.IsDelete);
            configuration.Property(c => c.CreationDate);
            configuration.Property(c => c.AccountId);
            configuration.Property(c => c.Address);
            configuration.Property(c => c.Age);
            configuration.Property(c => c.AboutMe);
            configuration.Property(c => c.Avatar);
            configuration.Property(c => c.BannerImg);
            configuration.Property(c => c.DisplayName);
            configuration.Property(c => c.DownVotes);
            configuration.Property(c => c.DoB);
            configuration.Property(c => c.Email);
            configuration.Property(c => c.Reputation);
            configuration.Property(c => c.Rank);
            configuration.Property(c => c.Location);
            configuration.Property(c => c.LastAccessDate);
            configuration.Property(c => c.LastName);
            configuration.Property(c => c.ProfileImageUrl);
            configuration.Property(c => c.Phone);
            configuration.Property(c => c.Password);
            configuration.Property(c => c.PostsCount);
            configuration.Property(c => c.FirstName);
            configuration.Property(c => c.FollowedCount);
            configuration.Property(c => c.UpVotes);
            configuration.Property(c => c.Status);
            configuration.Property(c => c.Views);
            configuration.Property(c => c.WebsiteUrl);
        }
    }
}
