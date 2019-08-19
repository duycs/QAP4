using Microsoft.Extensions.DependencyInjection;

namespace QAP4.Infrastructure.CrossCutting
{
    public class NativeInjectorBootStrapper
    {
        public static void Register(IServiceCollection services)
        {
            // Adding dependencies from another layers (isolated from Presentation)
            ApplicationLayerInjector.Register(services);
            DomainLayerInjector.Register(services);
            InfrastructureLayerInjector.Register(services);
        }
    }
}
