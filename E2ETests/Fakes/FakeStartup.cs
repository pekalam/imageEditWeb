using Autofac;
using ImageEdit.WebAPI;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Microsoft.Extensions.Configuration;

namespace E2ETests.Fakes
{
    public class FakeStartup : Startup
    {
        public FakeStartup(IConfiguration configuration) : base(configuration)
        {
        }

        protected override void ConfigureContainerModules(ContainerBuilder builder)
        {
            builder.RegisterModule(new FakeCoreModule());
        }
    }
}