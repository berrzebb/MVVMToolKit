using Microsoft.Extensions.DependencyInjection;
using MVVMToolKitSample.ViewModels;
using System;

namespace MVVMToolKitSample.Locator
{
    public class ViewModelLocator : IViewModelLocator
    {
        public IServiceProvider Container { get; }

        public MainWindowViewModel Main => Container.GetRequiredService<MainWindowViewModel>();


        public ViewModelLocator(IServiceProvider container)
        {
            Container = container;
        }
    }
}
