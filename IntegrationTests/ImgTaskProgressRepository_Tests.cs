using System;
using System.Linq;
using Autofac;
using AutoMapper;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;
using ImageEdit.Core.RegisterModules;
using Xunit;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Extensions.Configuration;
using Moq;
using TestUtils;

namespace IntegrationTests
{
    public class FakeDbModule : DbModule
    {

    }

    public class FakeCoreModule : CoreModule
    {
        protected override void RegisterBusModule(ContainerBuilder builder)
        {
        }

        protected override void RegisterDbModule(ContainerBuilder builder)
        {
            builder.RegisterModule<FakeDbModule>();
        }
    }

    public class ImgTaskProgressRepository_Tests : IDisposable
    {
        private readonly IImgTaskProgressRepository _imgTaskProgressRepository;
        private readonly ImageEditAppContext _dbContext;
        private readonly IContainer _container;
        private readonly IMapper _mapper;

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

        public ImgTaskProgressRepository_Tests()
        {
            _container = CreateDIContainer();
            _dbContext = _container.Resolve<ImageEditAppContext>();
            _imgTaskProgressRepository = _container.Resolve<IImgTaskProgressRepository>();
            _mapper = _container.Resolve<IMapper>();
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
        public void AddTaskProgress_adds_to_db()
        {
            var editTaskProgress = new ImgTaskProgress(Guid.NewGuid(), Guid.NewGuid(), ImgTaskState.Pending);
            var result = new ImgTaskResult(editTaskProgress.TaskId, editTaskProgress.GroupId, null, null);

            _dbContext.DbImgTaskResult.Add(_mapper.Map<ImgTaskResult, DbImgTaskResult>(result));

            _imgTaskProgressRepository.AddTaskProgress(editTaskProgress);

            var found = _dbContext.DbImgTaskProgress.SingleOrDefault(e =>
                e.TaskId == editTaskProgress.TaskId && e.GroupId == editTaskProgress.GroupId);
            var total = _dbContext.DbImgTaskProgress.Count();

            found.Should().NotBeNull();
            found.Should().BeEquivalentTo(editTaskProgress);
            total.Should().Be(1);
        }

        [Fact]
        public void UpdateTaskProgress_updates_taskProgress()
        {
            var editTaskProgress = new ImgTaskProgress(Guid.NewGuid(), Guid.NewGuid(), ImgTaskState.Pending);
            var result = new ImgTaskResult(editTaskProgress.TaskId, editTaskProgress.GroupId, null, null);

            _dbContext.DbImgTaskResult.Add(_mapper.Map<ImgTaskResult, DbImgTaskResult>(result));
            _dbContext.DbImgTaskProgress.Add(_mapper.Map<DbImgTaskProgress>(editTaskProgress));
            _dbContext.SaveChanges();

            editTaskProgress.ImgTaskState = ImgTaskState.Error;
            _imgTaskProgressRepository.UpdateTaskProgress(editTaskProgress);

            var found = _dbContext.DbImgTaskProgress.SingleOrDefault(e =>
                 e.GroupId == editTaskProgress.GroupId && e.TaskId == editTaskProgress.TaskId);
            var total = _dbContext.DbImgTaskProgress.Count();

            found.Should().NotBeNull();
            found.Should().BeEquivalentTo(editTaskProgress);
            total.Should().Be(1);
        }


        [Fact]
        public void GetTaskProgressesByGroupId_returs_valid_taskProgress()
        {
            var editTaskProgress = new ImgTaskProgress(Guid.NewGuid(), Guid.NewGuid(), ImgTaskState.Pending);
            var result = new ImgTaskResult(editTaskProgress.TaskId, editTaskProgress.GroupId, null, null);
            
            _dbContext.DbImgTaskResult.Add(_mapper.Map<ImgTaskResult, DbImgTaskResult>(result));
            _dbContext.DbImgTaskProgress.Add(_mapper.Map<DbImgTaskProgress>(editTaskProgress));
            _dbContext.SaveChanges();

            var found = _imgTaskProgressRepository.GetTaskProgressesByGroupId(editTaskProgress.GroupId);

            found.Length.Should().Be(1);
            found[0].Should().BeEquivalentTo(editTaskProgress);
        }

        public void Dispose()
        {
            TruncTables();
        }
    }
}