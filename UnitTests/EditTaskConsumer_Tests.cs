using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageEdit.Core.Domain;
using ImageEdit.Core.MessageConsumers;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Services;
using Moq;
using Xunit;

namespace UnitTests
{
    public class EditTaskConsumer_Tests
    {
        [Fact]
        public void f()
        {
            var stubImg = new MemoryStream(new byte[] { 1, 2, 3 });

            var editTask = new EditTask(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid(), new EditTaskAction("convert",
                new Dictionary<string, string>()
                {
                    {"from", "png"},
                    {"to", "pdf"}
                }));
            var expectedTaskResult = new EditTaskResult(editTask.Id, editTask.GroupId, stubImg, "png");

            var resultRepo = new Mock<IEditTaskResultRepository>();
            resultRepo.Setup(f => f.AddTaskResult(It.Is<EditTaskResult>(result =>
                result.TaskId == expectedTaskResult.TaskId && result.GroupId == expectedTaskResult.GroupId)));
            var imgRepo = new Mock<IImgRepository>();
            imgRepo.Setup(f => f.GetImg(editTask.ImgId)).Returns(stubImg);

            var executeService = new Mock<IEditTaskExecutorService>();
            executeService.Setup(f => f.ExecuteTask(editTask));


            var consumer = new EditTaskConsumer(resultRepo.Object, imgRepo.Object, executeService.Object);

            consumer.ProccessTask(editTask);



            resultRepo.Verify(f => f.AddTaskResult(It.Is<EditTaskResult>(result =>
                result.TaskId == expectedTaskResult.TaskId && result.GroupId == expectedTaskResult.GroupId)), Times.Once());
            executeService.Verify(f => f.ExecuteTask(editTask), Times.Once());
        }
    }
}