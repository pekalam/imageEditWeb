using System;
using System.Linq;
using AutoMapper;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;
using ImageEdit.Core.Repositories.Abstr;
using Microsoft.EntityFrameworkCore;

namespace ImageEdit.Core.Repositories
{
    public class ImgTaskResultRepository : IImgTaskResultRepository
    {
        private readonly ImageEditAppContext _dbContext;
        private readonly IMapper _mapper;

        public ImgTaskResultRepository(ImageEditAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ImgTaskResult GeTaskResult(Guid groupId, Guid taskId)
        {
            var found = _dbContext.DbImgTaskResult.AsNoTracking().FirstOrDefault(result => result.TaskId == taskId && result.GroupId == groupId);
            var taskResult = _mapper.Map<DbImgTaskResult, ImgTaskResult>(found);
            return taskResult;
        }

        public void AddTaskResult(ImgTaskResult result)
        {
            var dbEditTaskResult = _mapper.Map<ImgTaskResult, DbImgTaskResult>(result);

            _dbContext.DbImgTaskResult.Add(dbEditTaskResult);
            _dbContext.SaveChanges();
        }

        public void UpdateTaskResult(ImgTaskResult result)
        {
            var dbEditTaskResult = _mapper.Map<ImgTaskResult, DbImgTaskResult>(result);

            var existing = _dbContext.DbImgTaskResult.Find(result.GroupId, result.TaskId);
            existing.Image = dbEditTaskResult.Image;
            existing.Extension = dbEditTaskResult.Extension;

            _dbContext.DbImgTaskResult.Update(existing);

            _dbContext.SaveChanges();
        }
    }
}