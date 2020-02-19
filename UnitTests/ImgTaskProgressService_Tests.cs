using System;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using ImageEdit.Core.Services;
using Moq;
using Xunit;

namespace UnitTests
{
    public class ImgTaskProgressService_Tests
    {
        [Fact]
        public void GetTaskProgressGroup_when_valid_groupId_returns_progress_group()
        {
            var groupId = Guid.NewGuid();
            var progresses = new ImgTaskProgress[]
            {
                new ImgTaskProgress(Guid.NewGuid(), groupId, ImgTaskState.Pending),
            };
            var mockRepo = new Mock<IImgTaskProgressRepository>();
            mockRepo.Setup(f => f.GetTaskProgressesByGroupId(groupId)).Returns(() => progresses);
            var service = new ImgTaskProgressService(mockRepo.Object);

            var returned = service.GetTaskProgressGroup(groupId);

            mockRepo.Verify(f => f.GetTaskProgressesByGroupId(groupId), Times.Once());
            mockRepo.Verify(f => f.AddTaskProgress(It.IsAny<ImgTaskProgress>()), Times.Never());
        }

        [Fact]
        public void GetTaskProgressGroup_when_progress_does_not_exist_returns_null()
        {
            var groupId = Guid.NewGuid();

            var mockRepo = new Mock<IImgTaskProgressRepository>();
            mockRepo.Setup(f => f.GetTaskProgressesByGroupId(groupId)).Returns(() => null);
            var service = new ImgTaskProgressService(mockRepo.Object);

            service.GetTaskProgressGroup(groupId).Should().BeNull();
        }
    }
}