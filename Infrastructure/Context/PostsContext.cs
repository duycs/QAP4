using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using QAP4.Domain.AggreatesModels.Posts.Models;
using QAP4.Domain.AggreatesModels.Users.Models;
using QAP4.Infrastructure.EntityConfigurations;

namespace QAP4.Infrastructure.Context
{
    public class PostsContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Posts> Posts { get; set; }
        public DbSet<PostsLink> PostsLinks { get; set; }
        public DbSet<PostsTag> PostsTags { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Vote> Votes { get; set; }
        public DbSet<User> Users { get; set; }

        public PostsContext(DbContextOptions<PostsContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //model builder
            modelBuilder.ApplyConfiguration(new CommentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PostsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PostsLinkEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PostsEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PostsTagEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new QuoteEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new TagEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new UserEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VoteEntityTypeConfiguration());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            // get the configuration from the app settings
            var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();

            // define the database to use
            //use sql server
            optionsBuilder.UseSqlServer(config.GetConnectionString("DefaultConnection"));
        }

    }
}