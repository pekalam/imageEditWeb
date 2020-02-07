using AutoMapper;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Entities;

namespace ImageEdit.Core.Persistence.Mapping
{
    public class DbImgProfile : Profile
    {
        public DbImgProfile()
        {
            CreateMap<DbImg, Img>();
            CreateMap<Img, DbImg>();
        }
    }
}
