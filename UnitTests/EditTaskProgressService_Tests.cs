using System;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Services;
using ImageEdit.Core.Services.Impl;
using Moq;
using Xunit;

namespace UnitTests
{
    public class EditTaskProgressService_Tests
    {
        [Fact]
        public void GetTaskProgressGroup_when_valid_groupId_returns_progress_group()
        {
            var groupId = Guid.NewGuid();
            var progresses = new EditTaskProgress[]
            {
                new EditTaskProgress(Guid.NewGuid(), groupId, EditTaskState.Pending),
            };
            var mockRepo = new Mock<IEditTaskProgressRepository>();
            mockRepo.Setup(f => f.GetTaskProgressesByGroupId(groupId)).Returns(() => progresses);
            var service = new EditTaskProgressService(mockRepo.Object);

            var returned = service.GetTaskProgressGroup(groupId);

            mockRepo.Verify(f => f.GetTaskProgressesByGroupId(groupId), Times.Once());
            mockRepo.Verify(f => f.AddTaskProgress(It.IsAny<EditTaskProgress>()), Times.Never());
        }

        [Fact]
        public void GetTaskProgressGroup_when_progress_does_not_exist_returns_null()
        {
            var groupId = Guid.NewGuid();

            var mockRepo = new Mock<IEditTaskProgressRepository>();
            mockRepo.Setup(f => f.GetTaskProgressesByGroupId(groupId)).Returns(() => null);
            var service = new EditTaskProgressService(mockRepo.Object);

            service.GetTaskProgressGroup(groupId).Should().BeNull();
        }
    }
}