
using Microsoft.Extensions.DependencyInjection;
using QAP4.Infrastructure.Context;
using QAP4.Infrastructure.Helpers.File;
using QAP4.Infrastructure.Helpers.Mail;
using QAP4.Models;
using QAP4.Repository;

namespace QAP4.Infrastructure.CrossCutting
{
    public class InfrastructureLayerInjector
    {
        public static void Register(IServiceCollection services)
        {
            // Infra - Data
            services.AddDbContext<QAPContext>();

            //unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            //Infra - repository
            services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
            
            //Infra  - services
            services.AddTransient<IEmailSender, EmailSender>();
            services.AddTransient<IAmazonS3Service, AmazonS3Service>();
        }
    }
}