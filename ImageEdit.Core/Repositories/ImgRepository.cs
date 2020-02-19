using System;
using System.IO;
using AutoMapper;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;
using ImageEdit.Core.Repositories.Abstr;

namespace ImageEdit.Core.Repositories
{
    internal class ImgRepository : IImgRepository
    {
        private readonly ImageEditAppContext _dbContext;
        private readonly IMapper _mapper;

        public ImgRepository(ImageEditAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public Img AddImg(Img img)
        {
            var dbImg = _mapper.Map<Img, DbImg>(img);

            var saved = _dbContext.DbImg.Add(dbImg);
            _dbContext.SaveChanges();
            return img;
        }

        public Img GetImg(Guid imgId)
        {
            var dbImg = _dbContext.DbImg.Find(imgId);
            var img = _mapper.Map<DbImg, Img>(dbImg);

            return img;
        }
    }
}