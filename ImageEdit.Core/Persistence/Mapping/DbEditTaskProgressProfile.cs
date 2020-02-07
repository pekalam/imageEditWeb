using AutoMapper;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Entities;

namespace ImageEdit.Core.Persistence.Mapping
{
    public class DbEditTaskProgressProfile : Profile
    {
        public DbEditTaskProgressProfile()
        {
            CreateMap<DbEditTaskProgress, EditTaskProgress>();
            CreateMap<EditTaskProgress, DbEditTaskProgress>();
        }
    }
}