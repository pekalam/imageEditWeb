using Autofac;
using ImageEdit.Core.MessageConsumers;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Module = Autofac.Module;

namespace ImageEdit.Core.RegisterModules
{
    public class BusModule : Module
    {
        protected virtual void RegisterConsumers(ContainerBuilder builder)
        {
            builder.RegisterConsumers(typeof(BusModule).Assembly);
        }

        protected virtual void RegisterEditTaskConsumers(IRabbitMqReceiveEndpointConfigurator configurator,
            IComponentContext context)
        {
            configurator.ConfigureConsumer<EditTaskConsumer>(context);
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterConsumers(builder);
            builder.AddMassTransit(configure =>
            {
                configure.AddBus(context => Bus.Factory.CreateUsingRabbitMq(cfg =>
                {
                    configure.AddConsumersFromContainer(context);

                    cfg.Host("localhost");

                    cfg.ReceiveEndpoint("editTask", configurator =>
                    {
                        RegisterEditTaskConsumers(configurator, context);
                    });
                }));
            });
        }
    }
}