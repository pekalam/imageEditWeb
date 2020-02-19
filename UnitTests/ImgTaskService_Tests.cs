using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImageEdit.Core.Domain;
using ImageEdit.Core.RegisterModules;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using ImageEdit.Core.Services;
using ImageEdit.Core.Services.Abstr;
using Moq;
using Xunit;

namespace UnitTests
{
    public class ImgTaskService_Tests
    {
        [Fact]
        public void CreateEditTask_when_called_sends_valid_msgs()
        {
            ImgTaskFactoryRegistry.Init(typeof(CoreModule).Assembly);

            //mocks
            var imgRepo = new Mock<IImgRepository>();
            var mqService = new Mock<IMessageQueueService>();
            var imgTaskProgressRepo = new Mock<IImgTaskProgressRepository>();
            var imgTaskResultRepo = new Mock<IImgTaskResultRepository>();
            //

            var service = new ImgTaskService(imgRepo.Object, mqService.Object, imgTaskProgressRepo.Object, imgTaskResultRepo.Object);
            var img = new MemoryStream(new byte[] {1, 2, 3});
            var creationParams1 = new ImgTaskCreationParams()
            {
                TaskName = "convert", TaskParams = new Dictionary<string, string>() { { "to", "jpg"} }
            };
            var creationParams2 = new ImgTaskCreationParams()
            {
                TaskName = "convert",
                TaskParams = new Dictionary<string, string>() { { "to", "pdf" } }
            };

            var progressId = service.CreateEditTask(img, "jpg", new ImgTaskCreationParams[] {creationParams1, creationParams2});

            imgTaskProgressRepo.Verify(f => f.AddTaskProgress(It.IsAny<ImgTaskProgress>()), Times.Exactly(2));
            mqService.Verify(f => f.QueueTaskAsync(It.IsAny<ImgTask>()), Times.Exactly(2));
            imgRepo.Verify(f => f.AddImg(It.IsAny<Img>()), Times.Once());
            imgTaskResultRepo.Verify(f => f.AddTaskResult(It.Is<ImgTaskResult>(result => result.ImgStream == null)), Times.Exactly(2));
        }
    }

    public class ImgTaskFactoryRegistry_Tests
    {
        [Fact]
        public void Register_when_called_twice_with_same_task_name_throws()
        {
            

            ImgTaskFactoryRegistry.Register("test",
                (id, imgId, @params) => new TestConvertTask(Guid.NewGuid(), id, imgId, @params));
            Assert.Throws<ArgumentException>(() =>
                ImgTaskFactoryRegistry.Register("test",
                    (id, imgId, @params) => new TestConvertTask(Guid.NewGuid(), id, imgId, @params)));
        }
    }
}