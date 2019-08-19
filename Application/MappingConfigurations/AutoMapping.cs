using AutoMapper;

namespace QAP4.Application.MappingConfigurations
{
    public class AutoMapping
    {
        public static MapperConfiguration RegisterMappings()
        {
            return new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new MappingEntityToDtoProfile());
                // cfg.AddProfile(new MappingViewModelToCommandProfile());
            });
        }
    }
}
