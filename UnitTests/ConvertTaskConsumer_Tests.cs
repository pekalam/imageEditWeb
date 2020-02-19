using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
using ImageEdit.Core.Domain;
using ImageEdit.Core.MessageConsumers;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using ImageEdit.Core.Services;
using Moq;
using Xunit;

namespace UnitTests
{
    public class TestConvertTask : ConvertImgTask
    {
        static TestConvertTask()
        {
            ImgTaskFactoryRegistry.Register("testConvert", (gid, imgId, @params) => new TestConvertTask(Guid.NewGuid(), gid, imgId, @params));
        }

        internal TestConvertTask(Guid taskId, Guid groupId, Guid imgId, Dictionary<string, string> taskParams) : base(taskId, groupId, imgId, taskParams)
        {
        }

        public override Img Execute(Img image)
        {
            return new Img(image.ImgId, new MemoryStream(new byte[]{1,2,3}), TaskParams["to"]);
        }
    }

    public class ConvertTaskConsumer_Tests
    {
        [Fact]
        public void ProcessTask_sends_valid_msgs_to_other_objects()
        {
            ImgTaskFactoryRegistry.Init(typeof(ConvertTaskConsumer_Tests).Assembly);

            var stubImg = new MemoryStream(new byte[] { 1, 2, 3 });

            var convertTask = (TestConvertTask)ImgTaskFactoryRegistry.GetImgTask(Guid.NewGuid(), Guid.NewGuid(), "testConvert",
                new Dictionary<string, string>()
                {
                    {"to", "pdf"}
                });
            var expectedTaskResult = new ImgTaskResult(convertTask.TaskId, convertTask.GroupId, stubImg, "png");

            var resultRepo = new Mock<IImgTaskResultRepository>();

            var imgRepo = new Mock<IImgRepository>();
            imgRepo.Setup(f => f.GetImg(convertTask.ImgId)).Returns(new Img(convertTask.ImgId, stubImg, "png"));

            var progressRepo = new Mock<IImgTaskProgressRepository>();

            var consumer = new ConvertTaskConsumer(resultRepo.Object, progressRepo.Object, imgRepo.Object);

            consumer.ProcessTask(convertTask);



            resultRepo.Verify(f => f.AddTaskResult(It.Is<ImgTaskResult>(result =>
                result.TaskId == expectedTaskResult.TaskId && result.GroupId == expectedTaskResult.GroupId)), Times.Never());
            resultRepo.Verify(f => f.UpdateTaskResult(It.Is<ImgTaskResult>(result =>
                result.TaskId == expectedTaskResult.TaskId && result.GroupId == expectedTaskResult.GroupId)), Times.Once());
        }
    }
}