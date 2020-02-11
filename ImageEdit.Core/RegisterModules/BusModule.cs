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
        protected virtual void ConfigureBus(IRabbitMqBusFactoryConfigurator cfg, IConfiguration configuration)
        {
            cfg.Host(configuration["RabbitMQ:Host"]);
        }

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
                    var config = context.Resolve<IConfiguration>();
                    
                    configure.AddConsumersFromContainer(context);
                    ConfigureBus(cfg, config);

                    cfg.ReceiveEndpoint("editTask", configurator =>
                    {
                        RegisterEditTaskConsumers(configurator, context);
                    });
                }));
            });
        }
    }
}