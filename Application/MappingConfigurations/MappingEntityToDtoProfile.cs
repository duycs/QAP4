using System;
using AutoMapper;
using QAP4.Application.DataTransferObjects;
using QAP4.Domain;
using QAP4.Domain.Core.Models;

namespace QAP4.Application.MappingConfigurations
{
    public class MappingEntityToDtoProfile : Profile
    {
        public MappingEntityToDtoProfile()
        {
            CreateMap<Entity, EntityDto>();

            //TODO: mapping share model not entity or viewModel
            CreateMap<FileUploadModel, FileDto>();

            //ForMember(dest => dest.PersonDto, act => act.MapFrom(src => src.Person));
        }
    }
}
