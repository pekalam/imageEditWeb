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
        protected virtual void ConfigureDbContextOptions(DbContextOptionsBuilder<ImageEditAppContext> optionsBuilder, IConfiguration configuration)
        {
            optionsBuilder.UseSqlServer(configuration["SQLServer:ConnectionString"]);
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.Register<DbContextOptions<ImageEditAppContext>>((IComponentContext context) =>
            {
                var config = context.Resolve<IConfiguration>();
                               
                var optionsBuilder = new DbContextOptionsBuilder<ImageEditAppContext>();
                ConfigureDbContextOptions(optionsBuilder, config);

                return optionsBuilder.Options;
            }).InstancePerLifetimeScope();

            builder.RegisterType<ImageEditAppContext>()
                .AsSelf()
                .InstancePerLifetimeScope();
        }
    }
}
