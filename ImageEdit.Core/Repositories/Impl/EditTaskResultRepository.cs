using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using GreenPipes.Internals.Mapping;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;

namespace ImageEdit.Core.Repositories.Impl
{
    public class EditTaskResultRepository : IEditTaskResultRepository
    {
        private readonly ImageEditAppContext _dbContext;
        private readonly IMapper _mapper;

        public EditTaskResultRepository(ImageEditAppContext dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public EditTaskResult GeTaskResult(Guid groupId, Guid taskId)
        {
            var found = _dbContext.DbEditTaskResult.FirstOrDefault(result => result.TaskId == taskId && result.GroupId == groupId);
            var taskResult = _mapper.Map<DbEditTaskResult, EditTaskResult>(found);
            return taskResult;
        }

        public void AddTaskResult(EditTaskResult result)
        {
            var dbEditTaskResult = _mapper.Map<EditTaskResult, DbEditTaskResult>(result);

            _dbContext.DbEditTaskResult.Add(dbEditTaskResult);
            _dbContext.SaveChanges();
        }
    }
}