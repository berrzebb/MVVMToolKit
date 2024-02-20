﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using MVVMToolKit.Hosting.Core;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.ViewModels
{
    /// <summary>
    /// The view model base class.
    /// </summary>
    /// <seealso cref="ViewModelBase"/>
    public abstract class ViewModelBase<TViewModel> : ViewModelBase
        where TViewModel : ViewModelBase
    {
        /// <summary>
        /// The logger.
        /// </summary>
        protected ILogger<TViewModel> Logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ViewModelBase"/> class.
        /// </summary>
        /// <param name="provider">The provider.</param>
        protected ViewModelBase(IServiceProvider provider)
        {
            this.currentProvider = provider;
            this.dispatcherService = this.currentProvider.GetRequiredService<IDispatcherService>();
            this.disposableObjectService = this.currentProvider.GetRequiredService<IDisposableObjectService>();
            this.Logger = this.currentProvider.GetRequiredService<ILogger<TViewModel>>();
            Logger.LogDebug($"{typeof(TViewModel).Name} Initialize.");
            this.Initialize(this.currentProvider);
        }

        private void Initialize(IServiceProvider provider)
        {

            this.InitializeDependency(provider);
        }

        /// <summary>
        /// Initializes the dependency using the specified container provider.
        /// </summary>
        /// <param name="containerProvider">The container provider.</param>
        protected override void InitializeDependency(IServiceProvider containerProvider)
        {
        }
    }
}
