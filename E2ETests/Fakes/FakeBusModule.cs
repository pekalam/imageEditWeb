using Autofac;
using ImageEdit.Core.RegisterModules;
using MassTransit;
using MassTransit.RabbitMqTransport;

namespace E2ETests.Fakes
{
    public class FakeBusModule : BusModule
    {
        protected override void RegisterConsumers(ContainerBuilder builder)
        {
            builder.RegisterConsumers(typeof(FakeBusModule).Assembly);
        }

        protected override void RegisterEditTaskConsumers(IRabbitMqReceiveEndpointConfigurator configurator, IComponentContext context)
        {
            configurator.ConfigureConsumer<FakeEditTaskConsumer>(context);
        }
    }
}