using System;
using Autofac;
using ImageEdit.Core.RegisterModules;
using Xunit;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Persistence.Entities;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using TestUtils;

namespace UnitTests
{
    public class CoreModule_Tests
    {
        private readonly IContainer _container;

        public CoreModule_Tests()
        {
            var builder = new ContainerBuilder();
            builder.Register<IConfiguration>(context => TestSettings.Configuration);
            builder.RegisterModule<CoreModule>();
            _container = builder.Build();
        }

        [Theory]
        [InlineData(typeof(ImageEditAppContext))]
        public void RequiredTypes_gets_registered(Type registeredType)
        {
            _container.Resolve(registeredType);
        }
    }
}
