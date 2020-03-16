
using Microsoft.Extensions.DependencyInjection;
using QAP4.Application.Services;

namespace QAP4.Infrastructure.CrossCutting
{
    public class DomainLayerInjector
    {
        public static void Register(IServiceCollection services)
        {
            //Distributed Interface Install

            // Domain Bus (Mediator)
            // services.AddScoped<ICommandDispatcher, CommandDispatcher>();
            // services.AddScoped<IEventDispatcher, EventDispatcher>();

            // Domain - Events
            // services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            // services.AddScoped<INotificationHandler<BookCreatedEvent>, DomainEventHandler<BookCreatedEvent>>();

            // Application Service
            services.AddTransient<PostsService>();


            // Domain - Services
            //services.AddTransient<Service>();
        }
    }
}