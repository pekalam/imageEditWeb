using Autofac;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.RegisterModules;
using Microsoft.EntityFrameworkCore;

namespace E2ETests.Fakes
{
    public class FakeDbModule : DbModule
    {
        protected override void ConfigureDbContextOptions(DbContextOptionsBuilder<ImageEditAppContext> optionsBuilder, IComponentContext context)
        {
            base.ConfigureDbContextOptions(optionsBuilder, context);
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=ImageEdit;User ID=sa;Password=qwerty");
        }
    }
}