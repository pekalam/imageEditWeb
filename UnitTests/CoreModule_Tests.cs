using System;
using Autofac;
using ImageEdit.Core.Domain;
using ImageEdit.Core.RegisterModules;
using Xunit;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Services.Impl;
using Microsoft.Extensions.Configuration;
using Moq;

namespace UnitTests
{
    public class CoreModule_Tests
    {
        private readonly IContainer _container;

        public CoreModule_Tests()
        {
            var builder = new ContainerBuilder();
            builder.Register<IConfiguration>(context => { return Mock.Of<IConfiguration>(); });
            builder.RegisterModule<CoreModule>();
            _container = builder.Build();
        }

        [Theory]
        [InlineData(typeof(ImageEditAppContext))]
        public void RequiredTypes_gets_registered(Type registeredType)
        {
            _container.Resolve(registeredType);
        }
    }

    public class EditTaskProgressService_Tests
    {
        public void GetTaskProgressGroup_when_valid_groupId_returns_progress_group()
        {
            var groupId = Guid.NewGuid();
            var progresses = new EditTaskProgress[]
            {
                new EditTaskProgress(Guid.NewGuid(), groupId, EditTaskState.Pending),
            };
            var mockRepo = new Mock<IEditTaskProgressRepository>();
            mockRepo.Setup(f => f.GetTaskProgressesByGroupId(groupId)).Returns(() =>
            {
                return progresses;
            });
            var service = new EditTaskProgressService(mockRepo.Object);

            var returned = service.GetTaskProgressGroup(groupId);

            mockRepo.Verify(f => f.GetTaskProgressesByGroupId(groupId), Times.Once());
            mockRepo.Verify(f => f.AddTaskProgress(It.IsAny<EditTaskProgress>()), Times.Never());
        }
    }
}
