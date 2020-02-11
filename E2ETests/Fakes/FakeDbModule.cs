using Autofac;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.RegisterModules;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using TestUtils;

namespace E2ETests.Fakes
{
    public class FakeDbModule : DbModule
    {
    }
}