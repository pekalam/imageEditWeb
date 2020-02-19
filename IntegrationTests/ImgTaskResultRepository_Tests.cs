using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using Autofac;
using AutoMapper;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using IntegrationTests;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using TestUtils;
using Xunit;

[assembly: CollectionBehavior(DisableTestParallelization = true)]

namespace IntegrationTests
{
    public class ImgTaskResultRepository_Tests : IDisposable
    {
        private readonly IImgTaskResultRepository _imgTaskResultRepository;
        private readonly ImageEditAppContext _dbContext;
        private readonly IContainer _container;


        private void TruncTables()
        {
            const string constraintName = "FK_DbImgTaskProgress_DbImgTaskResult";
            _dbContext.Database.ExecuteSqlRaw($"ALTER TABLE [{nameof(_dbContext.DbImgTaskProgress)}] " +
                                              $"DROP CONSTRAINT {constraintName};");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(_dbContext.DbImgTaskResult)}];");
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(_dbContext.DbImgTaskProgress)}];");
            _dbContext.Database.ExecuteSqlRaw($"ALTER TABLE [{nameof(_dbContext.DbImgTaskProgress)}] " +
                                              $"ADD CONSTRAINT {constraintName} FOREIGN KEY ([GroupId], [TaskId]) " +
                                              $"REFERENCES [dbo].[{nameof(_dbContext.DbImgTaskResult)}]([GroupId], [TaskId])");
        }

        public ImgTaskResultRepository_Tests()
        {
            _container = CreateDIContainer();
            _dbContext = _container.Resolve<ImageEditAppContext>();
            _imgTaskResultRepository = _container.Resolve<IImgTaskResultRepository>();
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


            var progress = new ImgTaskProgress(taskId, groupId, ImgTaskState.Completed);
            var result = new ImgTaskResult(taskId, groupId, new MemoryStream(new byte[] {1, 2, 3}), "png");

            _dbContext.DbImgTaskProgress.Add(mapper.Map<ImgTaskProgress, DbImgTaskProgress>(progress));
            _dbContext.DbImgTaskResult.Add(mapper.Map<ImgTaskResult, DbImgTaskResult>(result));
            _dbContext.SaveChanges();

            var foundResult = _imgTaskResultRepository.GeTaskResult(groupId, taskId);

            foundResult.ImgStream.Length.Should().BeGreaterThan(0);
            foundResult.ImgStream.CanRead.Should().BeTrue();
            foundResult.Should().BeEquivalentTo(result,
                options => { return options.Excluding(taskResult => taskResult.ImgStream); });
        }

        [Fact]
        public void AddTaskResult_when_task_progress_exist_saves_task_result()
        {
            var groupId = Guid.NewGuid();
            var taskId = Guid.NewGuid();

            var result = new ImgTaskResult(taskId, groupId, new MemoryStream(new byte[] {1, 2, 3}), "png");

            _imgTaskResultRepository.AddTaskResult(result);

            _dbContext.DbImgTaskResult.Count().Should().Be(1);
        }

        [Fact]
        public void UpdateTaskResult_when_task_exists_updates_it()
        {
            var groupId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var mapper = _container.Resolve<IMapper>();


            var result = new ImgTaskResult(taskId, groupId, null, null);

            _dbContext.DbImgTaskResult.Add(mapper.Map<ImgTaskResult, DbImgTaskResult>(result));
            _dbContext.SaveChanges();


            var updated = new ImgTaskResult(taskId, groupId, new MemoryStream(new byte[] {1, 2, 3}), "png");

            _imgTaskResultRepository.UpdateTaskResult(updated);


            var foundResult = _dbContext.DbImgTaskResult.Find(groupId, taskId);

            foundResult.Image.Length.Should().Be(3);
            foundResult.Should().BeEquivalentTo(updated,
                options => { return options.Excluding(taskResult => taskResult.ImgStream); });
        }

        public void Dispose()
        {
            TruncTables();
        }
    }
}