﻿using System.Windows;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Navigation.Mapping;

namespace ProtoTypeModule
{
    public class ProtoModule : IModule
    {
        /// <inheritdoc />
        public void ConfigureMappings(IMappingRegistry registry)
        {
        }

        public Task Initialize(IServiceProvider provider)
        {
            MessageBox.Show("Initlaize Proto");
            return Task.CompletedTask;

        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IPrototypeInterface, ProtoTypeInterface>();
        }

    }
}
