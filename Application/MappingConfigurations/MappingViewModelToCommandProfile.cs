using System;
using AutoMapper;

namespace QAP4.Application.MappingConfigurations
{
    public class MappingViewModelToCommandProfile : Profile
    {
        public MappingViewModelToCommandProfile()
        {
            // //posts
            // CreateMap<AddNewPostViewModel, CreatePostCommand>()
            //     .ConstructUsing(c => new CreatePostCommand(
            //         c.Uid));
        }
    }
}
