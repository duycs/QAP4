using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using QAP4.Application.Services;

namespace QAP4.Infrastructure.CrossCutting
{
    public class ApplicationLayerInjector
    {
        public static void Register(IServiceCollection services)
        {
            //Application
            //services.AddSingleton(Mapper.Configuration);
            services.AddScoped<IMapper>(sp => new Mapper(sp.GetRequiredService<IConfigurationProvider>(), sp.GetService));

            // services.AddScoped<IAuthenticationService, AuthenticationService>();
            services.AddScoped<ICommentService, CommentService>();
            services.AddScoped<ISearchService, SearchService>();

            // Application Service
            // services.AddTransient<BookService>();
        }
    }
}