using QAP4.Domain.Core.Repositories;
using QAP4.Infrastructure.Context;
using Microsoft.Extensions.DependencyInjection;
using QAP4.Domain.Extensions.Mail;
using QAP4.Infrastructure.Extensions.File;
using QAP4.Infrastructure.Repositories;
using QAP4.Models;

namespace QAP4.Infrastructure.CrossCutting
{
    public class InfrastructureLayerInjector
    {
        public static void Register(IServiceCollection services)
        {
            // Infra - Data
            // services.AddDbContext<EventContext>();
            services.AddDbContext<QAPContext>();

            //unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Infra - repository
            services.AddScoped(typeof(IRepository<>), typeof(EFRepository<>));
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IPostsRepository, PostsRepository>();
            services.AddTransient<ITagRepository, TagRepository>();
            services.AddTransient<IPostsTagRepository, PostsTagRepository>();
            services.AddTransient<ICommentRepository, CommentRepository>();
            services.AddTransient<IQuoteRepository, QuotesRepository>();
            services.AddTransient<IVoteRepository, VoteRepository>();
            services.AddTransient<IPostLinkRepository, PostLinkReposity>();

            //Infra  - services
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IAmazonS3Service, AmazonS3Service>();
        }
    }
}