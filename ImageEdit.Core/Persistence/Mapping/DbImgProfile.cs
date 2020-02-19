using System.IO;
using AutoMapper;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Entities;

namespace ImageEdit.Core.Persistence.Mapping
{
    public class DbImgProfile : Profile
    {
        public DbImgProfile()
        {
            CreateMap<DbImg, Img>()
                .ForMember(img => img.ImageStream, opt =>
                {
                    opt.MapFrom<Stream>(db => MappingHelpers.MapToStream(db.Image));
                });
            CreateMap<Img, DbImg>()
                .ForMember(dbImg => dbImg.Image, opt =>
                {
                    opt.MapFrom(img => MappingHelpers.MapToByteArr(img.ImageStream));
                });
        }
    }
}
