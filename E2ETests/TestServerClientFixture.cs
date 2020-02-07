using System.IO;
using System.Net.Http;
using Autofac.Extensions.DependencyInjection;
using E2ETests.Fakes;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.PlatformAbstractions;

namespace E2ETests
{
    public class TestServerClientFixture
    {
        public HttpClient Client { get; }

        public TestServerClientFixture()
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath;
            var setDir = Path.GetFullPath(Path.Combine(path));


            var builder = Host.CreateDefaultBuilder(null)
                .UseServiceProviderFactory(new AutofacServiceProviderFactory())
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseContentRoot(setDir)
                        .UseEnvironment("Development")
                        .UseStartup<FakeStartup>()
                        .UseTestServer();
                })
                .Start();

            Client = builder.GetTestClient();
        }
    }
}