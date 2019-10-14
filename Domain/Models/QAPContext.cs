using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace QAP4.Models
{
    public partial class QAPContext : DbContext
    {
        public virtual DbSet<Badges> Badges { get; set; }
        public virtual DbSet<Certified> Certified { get; set; }
        public virtual DbSet<CloseAsOffTopicReasonTypes> CloseAsOffTopicReasonTypes { get; set; }
        public virtual DbSet<CloseReasonTypes> CloseReasonTypes { get; set; }
        public virtual DbSet<Comments> Comments { get; set; }
        public virtual DbSet<FlagTypes> FlagTypes { get; set; }
        public virtual DbSet<Following> Following { get; set; }
        public virtual DbSet<Images> Images { get; set; }
        public virtual DbSet<PostFeedback> PostFeedback { get; set; }
        public virtual DbSet<PostHistoryTypes> PostHistoryTypes { get; set; }
        public virtual DbSet<PostLinks> PostLinks { get; set; }
        public virtual DbSet<PostType> PostType { get; set; }
        public virtual DbSet<Posts> Posts { get; set; }
        public virtual DbSet<PostsTag> PostsTag { get; set; }
        public virtual DbSet<QuestionTag> QuestionTag { get; set; }
        public virtual DbSet<Questions> Questions { get; set; }
        public virtual DbSet<Quotes> Quotes { get; set; }
        public virtual DbSet<ReviewTaskResultType> ReviewTaskResultType { get; set; }
        public virtual DbSet<ReviewTaskResults> ReviewTaskResults { get; set; }
        public virtual DbSet<ReviewTaskStates> ReviewTaskStates { get; set; }
        public virtual DbSet<ReviewTaskTypes> ReviewTaskTypes { get; set; }
        public virtual DbSet<ReviewTasks> ReviewTasks { get; set; }
        public virtual DbSet<SuggestedEditVotes> SuggestedEditVotes { get; set; }
        public virtual DbSet<SuggestedEdits> SuggestedEdits { get; set; }
        public virtual DbSet<TagSynonyms> TagSynonyms { get; set; }
        public virtual DbSet<Tags> Tags { get; set; }
        public virtual DbSet<TestQuestion> TestQuestion { get; set; }
        public virtual DbSet<TestType> TestType { get; set; }
        public virtual DbSet<Tests> Tests { get; set; }
        public virtual DbSet<UserTest> UserTest { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<VoteTypes> VoteTypes { get; set; }
        public virtual DbSet<Votes> Votes { get; set; }

        // Unable to generate entity type for table 'dbo.ReviewRejectionReasons'. Please see the warning messages.
        // Unable to generate entity type for table 'dbo.PendingFlags'. Please see the warning messages.

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }
        public QAPContext(DbContextOptions<QAPContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Badges>(entity =>
            {
                entity.Property(e => e.Date).HasColumnType("date");

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Certified>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<CloseAsOffTopicReasonTypes>(entity =>
            {
                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DeactivationDate).HasColumnType("datetime");

                entity.Property(e => e.MarkdownMini).HasMaxLength(50);
            });

            modelBuilder.Entity<CloseReasonTypes>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Comments>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.ModificationDate).HasColumnType("datetime");

                entity.Property(e => e.ProfilePictureUrl).HasMaxLength(250);

                entity.Property(e => e.UserDisplayName).HasMaxLength(50);
            });

            modelBuilder.Entity<FlagTypes>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(50);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Following>(entity =>
            {
                entity.HasKey(e => new { e.FollowingUserId, e.FollowedUserId })
                    .HasName("PK_Following");
            });

            modelBuilder.Entity<Images>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<PostFeedback>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PostHistoryTypes>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<PostLinks>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<PostType>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Posts>(entity =>
            {
                entity.Property(e => e.CloseDate).HasColumnType("datetime");

                entity.Property(e => e.CommunityOwnedDate).HasColumnType("datetime");

                entity.Property(e => e.CoverImg).HasColumnType("varchar(max)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DeletionDate).HasColumnType("datetime");

                entity.Property(e => e.HeadContent).HasMaxLength(500);

                entity.Property(e => e.LastActivityDate).HasColumnType("datetime");

                entity.Property(e => e.LastEditDate).HasColumnType("datetime");

                entity.Property(e => e.RelatedPosts).HasColumnType("varchar(500)");

                entity.Property(e => e.Tags).HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(250);

                entity.Property(e => e.UserDisplayName).HasMaxLength(50);

                entity.Property(e => e.UserAvatar).HasColumnType("varchar(max)");
            });

            modelBuilder.Entity<PostsTag>(entity =>
            {
                entity.HasKey(e => new { e.TagId, e.PostsId })
                    .HasName("PK_PostsTag");
            });

            modelBuilder.Entity<QuestionTag>(entity =>
            {
                entity.HasKey(e => new { e.QuestionId, e.TagId })
                    .HasName("PK_QuestionTag");
            });

            modelBuilder.Entity<Questions>(entity =>
            {
                entity.Property(e => e.Answer1).HasMaxLength(250);

                entity.Property(e => e.Answer2).HasMaxLength(250);

                entity.Property(e => e.Answer3).HasMaxLength(250);

                entity.Property(e => e.Answer4).HasMaxLength(250);

                entity.Property(e => e.Question).HasMaxLength(500);

                entity.Property(e => e.Tags).HasMaxLength(500);
            });

            modelBuilder.Entity<Quotes>(entity =>
            {
                entity.Property(e => e.AuthorDisplayName).HasMaxLength(50);

                entity.Property(e => e.Content).HasMaxLength(500);

                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReviewTaskResultType>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ReviewTaskResults>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<ReviewTaskStates>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ReviewTaskTypes>(entity =>
            {
                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<ReviewTasks>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DeletionDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SuggestedEditVotes>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<SuggestedEdits>(entity =>
            {
                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.RejectionDate).HasColumnType("datetime");

                entity.Property(e => e.RevisionGuid).HasMaxLength(50);

                entity.Property(e => e.Tags).HasMaxLength(250);

                entity.Property(e => e.Title).HasMaxLength(250);
            });

            modelBuilder.Entity<TagSynonyms>(entity =>
            {
                entity.Property(e => e.ApprovalDate).HasColumnType("datetime");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.LastAutoRename).HasColumnType("datetime");

                entity.Property(e => e.SourceTagName).HasMaxLength(50);

                entity.Property(e => e.TargetTagName).HasMaxLength(50);
            });

            modelBuilder.Entity<Tags>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<TestQuestion>(entity =>
            {
                entity.HasKey(e => new { e.TestId, e.QuestionId })
                    .HasName("PK_TestQuestion");
            });

            modelBuilder.Entity<TestType>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Tests>(entity =>
            {
                entity.Property(e => e.CloseDate).HasColumnType("nchar(10)");

                entity.Property(e => e.CommentCount).HasColumnType("nchar(10)");

                entity.Property(e => e.Comments).HasColumnType("nchar(10)");

                entity.Property(e => e.CreationDate).HasColumnType("nchar(10)");

                entity.Property(e => e.DeletionDate).HasColumnType("nchar(10)");

                entity.Property(e => e.Description).HasMaxLength(500);

                entity.Property(e => e.LastActiveDate).HasColumnType("datetime");

                entity.Property(e => e.LastEditDate).HasColumnType("datetime");

                entity.Property(e => e.Tags).HasMaxLength(500);

                entity.Property(e => e.Title).HasMaxLength(500);

                entity.Property(e => e.UserDisplayName).HasMaxLength(50);
            });

            modelBuilder.Entity<UserTest>(entity =>
            {
                entity.Property(e => e.UserDisplayName).HasMaxLength(50);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.AboutMe).HasMaxLength(250);

                entity.Property(e => e.Address).HasMaxLength(250);

                entity.Property(e => e.Avatar).HasColumnType("varchar(max)");
                entity.Property(e => e.BannerImg).HasColumnType("varchar(max)");

                entity.Property(e => e.CreationDate).HasColumnType("datetime");

                entity.Property(e => e.DisplayName).HasMaxLength(50);

                entity.Property(e => e.DoB).HasColumnType("datetime");

                entity.Property(e => e.Email).HasColumnType("varchar(250)");

                entity.Property(e => e.FirstName).HasMaxLength(50);

                entity.Property(e => e.LastAccessDate).HasColumnType("datetime");

                entity.Property(e => e.LastName).HasMaxLength(50);

                entity.Property(e => e.Location).HasMaxLength(250);

                entity.Property(e => e.Password).HasMaxLength(250);

                entity.Property(e => e.Phone).HasColumnType("varchar(50)");

                entity.Property(e => e.ProfileImageUrl).HasMaxLength(250);

                entity.Property(e => e.Status).HasMaxLength(50);

                entity.Property(e => e.WebsiteUrl).HasMaxLength(250);
            });

            modelBuilder.Entity<VoteTypes>(entity =>
            {
                entity.Property(e => e.Name).HasMaxLength(50);
            });

            modelBuilder.Entity<Votes>(entity =>
            {
                entity.Property(e => e.CreationDate).HasColumnType("datetime");
            });
        }
    }
}