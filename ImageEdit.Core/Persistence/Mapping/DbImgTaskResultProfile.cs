using System;
using System.Collections.Generic;
using System.Text;
using AutoMapper;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Entities;
using MassTransit.Initializers.PropertyInitializers;

namespace ImageEdit.Core.Persistence.Mapping
{
    public class DbImgTaskResultProfile : Profile
    {
        public DbImgTaskResultProfile()
        {
            CreateMap<DbImgTaskResult, ImgTaskResult>()
                .ForMember(result => result.ImgStream,
                    opt => { opt.MapFrom(dbResult => MappingHelpers.MapToStream(dbResult.Image)); });

            CreateMap<ImgTaskResult, DbImgTaskResult>()
                .ForMember(dbResult => dbResult.DbImgTaskProgress, opt => opt.Ignore())
                .ForMember(dbResult => dbResult.Image, opt =>
                {
                    opt.MapFrom(result => MappingHelpers.MapToByteArr(result.ImgStream));
                });
        }


    }
}