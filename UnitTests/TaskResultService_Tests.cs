using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using ImageEdit.Core.Services;
using Moq;
using Xunit;

namespace UnitTests
{
    public class TaskResultService_Tests
    {
        public TaskResultService_Tests()
        {
        }

        [Fact]
        public void GetTaskResult_when_task_completed_returns_valid_result()
        {
            var groupId = Guid.NewGuid();
            var taskId = Guid.NewGuid();
            var expectedResult = new ImgTaskResult(taskId, groupId, new MemoryStream(new byte[3]), "png");
            var taskResultRepository = new Mock<IImgTaskResultRepository>();
            taskResultRepository.Setup(f => f.GeTaskResult(groupId, taskId))
                .Returns(expectedResult);

            var taskResultService = new ImgTaskResultService(taskResultRepository.Object);
            var result = taskResultService.GetTaskResult(groupId, taskId);

            result.GroupId.Should().Be(groupId);
            result.TaskId.Should().Be(taskId);
            result.ImgStream.CanRead.Should().BeTrue();
            result.ImgStream.Length.Should().BeGreaterThan(0);
            taskResultRepository.Verify(f => f.GeTaskResult(groupId, taskId), Times.Once());
        }
    }
}