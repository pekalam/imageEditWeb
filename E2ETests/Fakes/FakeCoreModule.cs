using Autofac;
using ImageEdit.Core.RegisterModules;

namespace E2ETests.Fakes
{
    public class FakeCoreModule : CoreModule
    {
        protected override void RegisterDbModule(ContainerBuilder builder)
        {
            builder.RegisterModule<FakeDbModule>();
        }

        protected override void RegisterBusModule(ContainerBuilder builder)
        {
            builder.RegisterModule<FakeBusModule>();
        }
    }
}