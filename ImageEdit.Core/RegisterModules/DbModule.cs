using System;
using System.Collections.Generic;
using System.Text;
using Autofac;
using ImageEdit.Core.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ImageEdit.Core.RegisterModules
{
    public class DbModule : Module
    {
        protected virtual void ConfigureDbContextOptions(DbContextOptionsBuilder<ImageEditAppContext> optionsBuilder, IComponentContext context)
        {
            var config = context.Resolve<IConfiguration>();
            optionsBuilder.UseSqlServer("Data Source=127.0.0.1;Initial Catalog=ImageEdit;User ID=sa;Password=qwerty");
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<DbContextOptions<ImageEditAppContext>>((IComponentContext context) =>
            {
                var optionsBuilder = new DbContextOptionsBuilder<ImageEditAppContext>();
                ConfigureDbContextOptions(optionsBuilder, context);

                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterType<ImageEditAppContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
