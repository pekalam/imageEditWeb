using System;
using AutoMapper;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Entities;
using RabbitMQ.Client.Framing.Impl;

namespace ImageEdit.Core.Persistence.Mapping
{
    public class DbImgTaskProgressProfile : Profile
    {
        public DbImgTaskProgressProfile()
        {
            CreateMap<DbImgTaskProgress, ImgTaskProgress>();
            CreateMap<ImgTaskProgress, DbImgTaskProgress>();
        }
    }
}