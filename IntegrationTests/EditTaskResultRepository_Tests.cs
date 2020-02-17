using System;
using System.IO;
using System.Linq;
using Autofac;
using AutoMapper;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;
using ImageEdit.Core.Repositories;
using IntegrationTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestUtils;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]
namespace IntegrationTests
{
    public class EditTaskResultRepository_Tests : IDisposable
    {
        private readonly IEditTaskResultRepository _editTaskResultRepository;
        private readonly ImageEditAppContext _dbContext;
        private readonly IContainer _container;


        private void TruncTables()
        {
            _dbContext.Database.ExecuteSqlRaw($"ALTER TABLE [{nameof(_dbContext.DbEditTaskResult)}] " +
                                              $"DROP CONSTRAINT FK_DbEditTaskResult_DbEditTaskProgress;");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(_dbContext.DbEditTaskResult)}];");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(_dbContext.DbEditTaskProgress)}];");
            _dbContext.Database.ExecuteSqlRaw($"ALTER TABLE [{nameof(_dbContext.DbEditTaskResult)}] " +
                                              $"ADD CONSTRAINT FK_DbEditTaskResult_DbEditTaskProgress FOREIGN KEY ([TaskId], [GroupId]) " +
                                              $"REFERENCES [dbo].[DbEditTaskProgress]([TaskId], [GroupId])");
        }

        public EditTaskResultRepository_Tests()
        {
            _container = CreateDIContainer();
            _dbContext = _container.Resolve<ImageEditAppContext>();
            _editTaskResultRepository = _container.Resolve<IEditTaskResultRepository>();
            TruncTables();   
        }

        private IContainer CreateDIContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register<IConfiguration>(context => TestSettings.Configuration);
            builder.RegisterModule<FakeCoreModule>();
            return builder.Build();
        }

        [Fact]
        public void GetTaskResult_when_task_exists_returns_valid_result()
        {
            var groupId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var mapper = _container.Resolve<IMapper>();


            var progress = new EditTaskProgress(taskId, groupId, EditTaskState.Completed);
            var result = new EditTaskResult(taskId, groupId, new MemoryStream(new byte[] { 1, 2, 3 }), "png");

            _dbContext.DbEditTaskProgress.Add(mapper.Map<EditTaskProgress, DbEditTaskProgress>(progress));
            _dbContext.DbEditTaskResult.Add(mapper.Map<EditTaskResult, DbEditTaskResult>(result));
            _dbContext.SaveChanges();

            var foundResult = _editTaskResultRepository.GeTaskResult(groupId, taskId);

            foundResult.ImgStream.Length.Should().BeGreaterThan(0);
            foundResult.ImgStream.CanRead.Should().BeTrue();
            foundResult.Should().BeEquivalentTo(result, options =>
                {
                    return options.Excluding(taskResult => taskResult.ImgStream);
                });
        }

        [Fact]
        public void AddTaskResult_when_task_progress_exist_saves_task_result()
        {
            var groupId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var mapper = _container.Resolve<IMapper>();

            
            var progress = new EditTaskProgress(taskId, groupId, EditTaskState.Completed);
            var result = new EditTaskResult(taskId, groupId, new MemoryStream(new byte[]{1,2,3}), "png");

            _dbContext.DbEditTaskProgress.Add(mapper.Map<EditTaskProgress, DbEditTaskProgress>(progress));

            _editTaskResultRepository.AddTaskResult(result);

            _dbContext.DbEditTaskResult.Count().Should().Be(1);
        }

        public void Dispose()
        {
            TruncTables();
        }
    }
}