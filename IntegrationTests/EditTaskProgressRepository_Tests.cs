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
using ImageEdit.Core.Repositories.Impl;
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

    public class EditTaskProgressRepository_Tests : IDisposable
    {
        private readonly IEditTaskProgressRepository _editTaskProgressRepository;
        private readonly ImageEditAppContext _dbContext;
        private readonly IContainer _container;

        public EditTaskProgressRepository_Tests()
        {
            _container = CreateDIContainer();
            _dbContext = _container.Resolve<ImageEditAppContext>();
            _editTaskProgressRepository = _container.Resolve<IEditTaskProgressRepository>();
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(_dbContext.DbEditTaskProgress)}];");
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
            var editTaskProgress = new EditTaskProgress(Guid.NewGuid(), Guid.NewGuid(), EditTaskState.Pending);
            _editTaskProgressRepository.AddTaskProgress(editTaskProgress);

            var found = _dbContext.DbEditTaskProgress.SingleOrDefault(e =>
                e.TaskId == editTaskProgress.TaskId && e.GroupId == editTaskProgress.GroupId);
            var total = _dbContext.DbEditTaskProgress.Count();

            found.Should().NotBeNull();
            found.Should().BeEquivalentTo(editTaskProgress);
            total.Should().Be(1);
        }

        [Fact]
        public void UpdateTaskProgress_updates_taskProgress()
        {
            var mapper = _container.Resolve<IMapper>();
            var editTaskProgress = new EditTaskProgress(Guid.NewGuid(), Guid.NewGuid(), EditTaskState.Pending);
            _dbContext.DbEditTaskProgress.Add(mapper.Map<DbEditTaskProgress>(editTaskProgress));
            _dbContext.SaveChanges();

            editTaskProgress.EditTaskState = EditTaskState.Error;
            _editTaskProgressRepository.UpdateTaskProgress(editTaskProgress);

            var found = _dbContext.DbEditTaskProgress.SingleOrDefault(e =>
                e.TaskId == editTaskProgress.TaskId && e.GroupId == editTaskProgress.GroupId);
            var total = _dbContext.DbEditTaskProgress.Count();

            found.Should().NotBeNull();
            found.Should().BeEquivalentTo(editTaskProgress);
            total.Should().Be(1);
        }

        public void Dispose()
        {
            _dbContext.Database.ExecuteSqlRaw($"TRUNCATE TABLE [{nameof(_dbContext.DbEditTaskProgress)}];");
        }
    }
}