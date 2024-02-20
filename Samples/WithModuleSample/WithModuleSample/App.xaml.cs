using System.Windows;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.Ioc.Modules;
using MVVMToolKit.Navigation.Mapping;
using ProtoTypeModule;

namespace WithModuleSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        /// <inheritdoc />
        protected override IModuleCatalog CreateModuleCatalog()
        {
            return new ModuleCatalog();
        }

        protected override void InitializeModuleCatalog(IModuleCatalog moduleCatalog)
        {
            moduleCatalog.AddModule(new ProtoModule());
        }

        protected override void InitializeViewModels(IServiceCollection services)
        {
            base.InitializeViewModels(services);
            services.AddViewModel<MainWindowModel>();
        }
        protected override void InitializeViews(IServiceCollection services)
        {
            base.InitializeViews(services);
            services.AddView<MainShell>();
            services.AddView<FirstView>(ServiceLifetime.Singleton);
            services.AddView<SecondaryView>();
            services.AddView<ThirdView>();
        }

        /// <inheritdoc />
        protected override void InitializeMappings(IMappingRegistry registry)
        {
            base.InitializeMappings(registry);

            // Method A View Selector Func
            //registry.AddMapping(new ViewConfiguration<MainWindowModel>()
            //{
            //    ViewMode = ViewMode.Selector,
            //    ViewSelector = vm =>
            //    {
            //        if (vm is MainWindowModel mvm)
            //        {
            //            switch (mvm.Selector)
            //            {
            //                case 0: return nameof(FirstView);
            //                case 1: return nameof(SecondaryView);
            //                default: return nameof(FirstView);
            //            }
            //        }

            //        return nameof(FirstView);
            //    },
            //    CacheMode = ViewCacheMode.DependencyInjection,
            //    ViewType = nameof(FirstView),
            //});
            // Method B View Selector Use Interface
            registry.Register(new MappingConfiguration<MainWindowModel>()
            {
                ViewMode = ViewMode.Selector
            });
            // Method C Single View
            //registry.Register(new MappingConfiguration<MainWindowModel>()
            //{
            //    ViewType = nameof(ThirdView),
            //    CacheMode = ViewCacheMode.DependencyInjection
            //});


        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var proto = ContainerProvider.Resolve<IPrototypeInterface>();

            proto.Print();
            MainWindow = ContainerProvider.Resolve<MainShell>();

            MainWindow.Content = ContainerProvider.Resolve<MainWindowModel>();
            MainWindow.Show();
        }
    }

}
