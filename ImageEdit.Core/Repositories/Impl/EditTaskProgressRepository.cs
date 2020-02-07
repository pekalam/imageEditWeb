using System;
using System.Linq;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;

namespace ImageEdit.Core.Repositories.Impl
{
    internal class EditTaskProgressRepository : IEditTaskProgressRepository
    {
        private readonly ImageEditAppContext _dbContext;
        private readonly IMapper _mapper;

        public EditTaskProgressRepository(ImageEditAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public EditTaskProgress[] GetTaskProgressesByGroupId(Guid groupId)
        {
            return _dbContext.DbEditTaskProgress
                .Where(progress => progress.GroupId == groupId)
                .ProjectTo<EditTaskProgress>(_mapper.ConfigurationProvider)
                .ToArray();
        }

        public void UpdateTaskProgress(EditTaskProgress editTask)
        {
            var dbModel = _mapper.Map<DbEditTaskProgress>(editTask);

            DbEditTaskProgress taskProgress = null;
//            if (_dbContext.DbEditTaskProgress.Local.FirstOrDefault(progress =>
//                    progress.TaskId == dbModel.TaskId && progress.GroupId == dbModel.GroupId) != null)
//            {
//                taskProgress = _dbContext.DbEditTaskProgress.Local.FirstOrDefault(progress =>
//                    progress.TaskId == dbModel.TaskId && progress.GroupId == dbModel.GroupId);
//            }
//            else
//            {
//                taskProgress = _dbContext.DbEditTaskProgress.FirstOrDefault(progress =>
//                    progress.TaskId == dbModel.TaskId && progress.GroupId == dbModel.GroupId);
//            }
            taskProgress = _dbContext.DbEditTaskProgress.Find(dbModel.TaskId, dbModel.GroupId);
            if (taskProgress == null)
            {
                throw new ArgumentException("Cannot find EditTaskProgress");
            }

            taskProgress.EditTaskState = dbModel.EditTaskState;

            _dbContext.DbEditTaskProgress.Update(taskProgress);
            _dbContext.SaveChanges();
        }

        public void AddTaskProgress(EditTaskProgress editTask)
        {
            var dbModel = _mapper.Map<DbEditTaskProgress>(editTask);
            _dbContext.DbEditTaskProgress.Add(dbModel);
            _dbContext.SaveChanges();
        }
    }
}