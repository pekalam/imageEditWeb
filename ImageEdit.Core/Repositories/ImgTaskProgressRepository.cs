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
    internal class ImgTaskProgressRepository : IImgTaskProgressRepository
    {
        private readonly ImageEditAppContext _dbContext;
        private readonly IMapper _mapper;

        public ImgTaskProgressRepository(ImageEditAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public ImgTaskProgress[] GetTaskProgressesByGroupId(Guid groupId)
        {
            var dbProgresses = _dbContext.DbImgTaskProgress
                .AsNoTracking()
                .Where(progress => progress.GroupId == groupId)
                .ToArray();


            return dbProgresses.Select(dbProgress => _mapper.Map<DbImgTaskProgress, ImgTaskProgress>(dbProgress)).ToArray();
        }

        public void UpdateTaskProgress(ImgTaskProgress imgTaskProgress)
        {
            var dbModel = _mapper.Map<DbImgTaskProgress>(imgTaskProgress);

            var taskProgress = _dbContext.DbImgTaskProgress.Find(dbModel.GroupId, dbModel.TaskId);
            if (taskProgress == null)
            {
                throw new ArgumentException($"Cannot find EditTaskProgress with key gid: {imgTaskProgress.GroupId}, taskId: {imgTaskProgress.TaskId}");
            }

            taskProgress.ImgTaskState = dbModel.ImgTaskState;

            _dbContext.DbImgTaskProgress.Update(taskProgress);
            _dbContext.SaveChanges();
        }

        public void AddTaskProgress(ImgTaskProgress imgTaskProgress)
        {
            var dbModel = _mapper.Map<DbImgTaskProgress>(imgTaskProgress);
            _dbContext.DbImgTaskProgress.Add(dbModel);
            _dbContext.SaveChanges();
        }
    }
}