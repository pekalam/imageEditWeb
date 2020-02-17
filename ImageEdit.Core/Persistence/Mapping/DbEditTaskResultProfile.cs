using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using AutoMapper;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Entities;
using MassTransit.Initializers.PropertyInitializers;

namespace ImageEdit.Core.Persistence.Mapping
{
    public class DbEditTaskResultProfile : Profile
    {
        public DbEditTaskResultProfile()
        {
            CreateMap<DbEditTaskResult, EditTaskResult>()
                .ForMember(result => result.ImgStream,
                    opt => { opt.MapFrom(dbResult => MapToStream(dbResult.Image)); });

            CreateMap<EditTaskResult, DbEditTaskResult>()
                .ForMember(dbResult => dbResult.DbEditTaskProgress, opt => opt.Ignore())
                .ForMember(dbResult => dbResult.Image, opt =>
                {
                    opt.MapFrom(result => MapToByteArr(result.ImgStream));
                });
        }

        private static Stream MapToStream(byte[] imgBytes)
        {
            var stream = new MemoryStream(imgBytes);
            return stream;
        }

        private static byte[] MapToByteArr(Stream stream)
        {
            using (MemoryStream mem = new MemoryStream())
            {
                stream.CopyTo(mem);
                return mem.ToArray();
            }
        }
    }
}