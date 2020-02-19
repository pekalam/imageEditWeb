using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.MessageConsumers;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.RegisterModules;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Abstr;
using ImageEdit.Core.Services;
using ImageEdit.Core.Services.Abstr;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using TestUtils;
using Xunit;

namespace IntegrationTests
{
    public class ConvertImageTestConsumer : ConvertTaskConsumer
    {
        public static int TimesCalled { get; set; }

        public override Task Consume(ConsumeContext<ConvertImgTask> context)
        {
            TimesCalled++;
            return base.Consume(context);
        }

        public ConvertImageTestConsumer(IImgTaskResultRepository imgTaskResultRepository,
            IImgTaskProgressRepository imgTaskProgress, IImgRepository imgRepository) : base(imgTaskResultRepository,
            imgTaskProgress, imgRepository)
        {
        }
    }

    public class ConvertImageTestBusModule : BusModule
    {
        protected override void RegisterEditTaskConsumers(IRabbitMqReceiveEndpointConfigurator configurator,
            IComponentContext context)
        {
            configurator.ConfigureConsumer<ConvertImageTestConsumer>(context);
        }

        protected override void RegisterConsumers(ContainerBuilder builder)
        {
            builder.RegisterConsumers(typeof(ConvertImageTestBusModule).Assembly);
        }
    }

    public class ConvertImageTestCoreModule : CoreModule
    {
        protected override void RegisterBusModule(ContainerBuilder builder)
        {
            builder.RegisterModule<ConvertImageTestBusModule>();
        }
    }

    public class ConvertImage_AcceptanceTest : IDisposable
    {
        private readonly IContainer _container;
        private readonly ImageEditAppContext _dbContext;
        private readonly ImgTaskService _editTaskService;
        private readonly ImgTaskProgressService _imgTaskProgressService;
        private readonly ImgTaskResultService _editImgTaskResultService;

        public ConvertImage_AcceptanceTest()
        {
            _container = CreateDIContainer();
            _dbContext = _container.Resolve<ImageEditAppContext>();
            var editTaskProgressRepository = _container.Resolve<IImgTaskProgressRepository>();
            var editTaskResultRepository = _container.Resolve<IImgTaskResultRepository>();
            var imgRepository = new ImgRepository(_dbContext, _container.Resolve<IMapper>());
            var mqService = _container.Resolve<IMessageQueueService>();
            _editTaskService = new ImgTaskService(imgRepository, mqService, editTaskProgressRepository, editTaskResultRepository);
            _imgTaskProgressService = new ImgTaskProgressService(editTaskProgressRepository);
            _editImgTaskResultService = new ImgTaskResultService(editTaskResultRepository);

            _container.Resolve<IBusControl>().Start(TimeSpan.FromSeconds(20));
        }

        private IContainer CreateDIContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register<IConfiguration>(context => TestSettings.Configuration);
            builder.RegisterModule<ConvertImageTestCoreModule>();
            return builder.Build();
        }

        private void WaitForTaskProgress(Guid groupId, Func<ImgTaskProgressGroup, bool> stopConditionFunc)
        {
            int i = 5;
            while (i > 0)
            {
                var progress = _imgTaskProgressService.GetTaskProgressGroup(groupId);

                if (stopConditionFunc(progress))
                {
                    return;
                }


                Thread.Sleep(2000);
                i--;
            }

            ConvertImageTestConsumer.TimesCalled.Should().Be(1);
            Assert.False(true, "Wait for task progress timed out");
        }

        private ImgTaskResult WaitForTaskResult(Guid groupId, Guid taskId)
        {
            int i = 5;
            while (i > 0)
            {
                var result = _editImgTaskResultService.GetTaskResult(groupId, taskId);

                if (result != null)
                {
                    return result;
                }

                Thread.Sleep(2000);
                i--;
            }

            Assert.True(false);
            return null;
        }

        [Fact]
        public void Convert_test()
        {
            ImgTaskFactoryRegistry.Init(typeof(CoreModule).Assembly);

            var img = File.OpenRead("img/0.png");
            var expected = File.OpenRead("img/0.converted.jpg");


            var creationParams = new ImgTaskCreationParams()
            {
                TaskName = "convert",
                TaskParams = new Dictionary<string, string>() {{"to", "jpg"}},
            };


            var groupId = _editTaskService.CreateEditTask(img, "png", new[] {creationParams});

            Guid taskId = new Guid();

            WaitForTaskProgress(groupId, progressGroup =>
            {
                progressGroup.GroupId.Should().Be(groupId);
                progressGroup.EditTasks.Length.Should().Be(1);
                progressGroup.EditTasks[0].GroupId.Should().Be(groupId);
                taskId = progressGroup.EditTasks[0].TaskId;
                return progressGroup.EditTasks.Count(progress => progress.ImgTaskState == ImgTaskState.Completed)
                       == progressGroup.EditTasks.Length;
            });

            var result = WaitForTaskResult(groupId, taskId);
            Assert.NotNull(result);

            if (result.ImgStream.Length != expected.Length)
            {
                Assert.False(true);
            }

            while (result.ImgStream.Position < result.ImgStream.Length)
            {
                if (result.ImgStream.ReadByte() != expected.ReadByte())
                {
                    Assert.False(true, "Images aren't the same");
                }
            }
        }


        public void Dispose()
        {
        }
    }


    public static class TestHelpers
    {
        public static bool CompareStreams(Stream first, Stream second)
        {
            if (first.Length != second.Length)
            {
                return false;
            }

            while (first.Position < first.Length)
            {
                if (first.ReadByte() != second.ReadByte())
                {
                    return false;
                }
            }

            return true;
        }
    }
}