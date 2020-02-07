﻿using System;
using Autofac;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using ImageEdit.Core.Persistence.Context;
using ImageEdit.Core.Services;
using ImageEdit.Core.Services.Impl;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ImageEdit.Core.RegisterModules
{
    public class CoreModule : Module
    {
        protected virtual void RegisterBusModule(ContainerBuilder builder)
        {
            builder.RegisterModule<BusModule>();
        }

        protected virtual void RegisterDbModule(ContainerBuilder builder)
        {
            builder.RegisterModule<DbModule>();
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterDbModule(builder);
            RegisterBusModule(builder);
            builder.RegisterAssemblyTypes(typeof(CoreModule).Assembly)
                .Where(t => t.Name.EndsWith("Repository"))
                .AsImplementedInterfaces();
            builder.RegisterType<ImageEditTaskService>().WithNonPublicCtors().InstancePerLifetimeScope();
            builder.RegisterType<EditTaskProgressService>().WithNonPublicCtors().InstancePerLifetimeScope();
            builder.RegisterType<MassTransitMqService>().WithNonPublicCtors().As<IMessageQueueService>()
                .InstancePerLifetimeScope();

            builder.AddAutoMapper(typeof(CoreModule).Assembly);
        }
    }
}