using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Castle.DynamicProxy.Generators.Emitters.SimpleAST;
using FluentAssertions;
using ImageEdit.Core.Domain;
using ImageEdit.Core.MessageConsumers;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.RegisterModules;
using ImageEdit.Core.Repositories;
using ImageEdit.Core.Repositories.Impl;
using ImageEdit.Core.Services;
using ImageEdit.Core.Services.Impl;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using TestUtils;
using Xunit;

namespace IntegrationTests
{
    public class ConvertImageTestConsumer : EditTaskConsumer
    {
        public static int TimesCalled { get; set; }

        public override Task Consume(ConsumeContext<EditTask> context)
        {
            TimesCalled++;
            return base.Consume(context);
        }

        public ConvertImageTestConsumer(IEditTaskResultRepository editTaskResultRepository, IImgRepository imgRepository, IEditTaskExecutorService editTaskExecutorService) : base(editTaskResultRepository, imgRepository, editTaskExecutorService)
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

    public class ConvertImage_Test : IDisposable
    {
        private readonly IContainer _container;
        private readonly ImageEditAppContext _dbContext;
        private readonly ImageEditTaskService _editTaskService;
        private readonly EditTaskProgressService _editTaskProgressService;
        private readonly TaskResultService _editTaskResultService;

        public ConvertImage_Test()
        {
            _container = CreateDIContainer();
            _dbContext = _container.Resolve<ImageEditAppContext>();
            var editTaskProgressRepository = _container.Resolve<IEditTaskProgressRepository>();
            var editTaskResultRepository = _container.Resolve<IEditTaskResultRepository>();
            var imgRepository = new ImgRepository();
            var mqService = _container.Resolve<IMessageQueueService>();
            _editTaskService = new ImageEditTaskService(imgRepository, mqService, editTaskProgressRepository);
            _editTaskProgressService = new EditTaskProgressService(editTaskProgressRepository);
            _editTaskResultService = new TaskResultService(editTaskResultRepository);

            _container.Resolve<IBusControl>().Start(TimeSpan.FromSeconds(20));
        }

        private IContainer CreateDIContainer()
        {
            var builder = new ContainerBuilder();
            builder.Register<IConfiguration>(context => TestSettings.Configuration);
            builder.RegisterModule<ConvertImageTestCoreModule>();
            return builder.Build();
        }

        private void WaitForTaskProgress(Guid groupId, Func<EditTaskProgressGroup, bool> stopConditionFunc)
        {
            int i = 5;
            while (i > 0)
            {
                var progress = _editTaskProgressService.GetTaskProgressGroup(groupId);

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

        [Fact]
        public void f()
        {
            var img = File.OpenRead("img/0.png");
            var expected = File.OpenRead("img/0.converted.jpg");


            var action = new EditTaskAction("convert", new Dictionary<string, string>()
            {
                {"from", "png"},
                {"to", "jpg"}
            });


            var groupId = _editTaskService.CreateEditTask(img, new EditTaskAction[]
            {
                action,
            });

            Guid taskId = new Guid();

            WaitForTaskProgress(groupId, progressGroup =>
            {
                progressGroup.GroupId.Should().Be(groupId);
                progressGroup.EditTasks.Length.Should().Be(1);
                progressGroup.EditTasks[0].GroupId.Should().Be(groupId);
                taskId = progressGroup.EditTasks[0].TaskId;
                return progressGroup.EditTasks.Count(progress => progress.EditTaskState == EditTaskState.Completed) 
                            == progressGroup.EditTasks.Length;
            });

            var result = _editTaskResultService.GetTaskResult(groupId, taskId);
            Assert.NotNull(result);

            while (result.ImgStream.CanRead || expected.CanRead)
            {
                if (result.ImgStream.ReadByte() != expected.ReadByte())
                {
                    Assert.False(true, "Images arent the smae");
                }
            }
        }


        public void Dispose()
        {
        }
    }
}