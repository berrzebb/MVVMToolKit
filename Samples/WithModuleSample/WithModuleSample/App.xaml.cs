using System.Windows;
using Infrastructure;
using Microsoft.Extensions.DependencyInjection;
using MVVMToolKit.Hosting.Extensions;
using MVVMToolKit.Interfaces;
using MVVMToolKit.Ioc;
using MVVMToolKit.Ioc.Modules;
using MVVMToolKit.Navigation.Mapping;
using ProtoTypeModule;
using WithModuleSample.ViewModels;
using WithModuleSample.Views;

namespace WithModuleSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public App()
        {
            SetDumpOption(true);
        }

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
            services.AddViewModel<SwitchViewModel>();
            services.AddViewModel<RegionableViewModel>();

            services.AddViewModel<WithPopupViewModel>();

        }
        protected override void InitializeViews(IServiceCollection services)
        {
            base.InitializeViews(services);
            services.AddView<MainShell>();
            services.AddView<FirstView>();
            services.AddView<SecondaryView>();
            services.AddView<ThirdView>();
            services.AddView<RegionableView>();

            services.AddView<GoldenView>();
            services.AddView<SilverView>();

            services.AddView<WithPopupView>();

            services.AddView<PopupContentView>();

        }

        /// <inheritdoc />
        protected override void InitializeMappings(IMappingRegistry registry)
        {
            base.InitializeMappings(registry);

            registry.Register(new MappingConfiguration()
            {
                RouteName = "Single",
                ViewMode = ViewMode.Single,
                ViewType = nameof(ThirdView)
            });
            registry.Register(new MappingConfiguration<SwitchViewModel>()
            {
                ViewMode = ViewMode.Selector
            });
            registry.Register(new MappingConfiguration<RegionableViewModel>()
            {
                RouteName = "Injection",
                ViewMode = ViewMode.Single,
                ViewType = nameof(RegionableView)
            });
            registry.Register(new MappingConfiguration()
            {
                RouteName = "Golden",
                ViewMode = ViewMode.Single,
                ViewType = nameof(GoldenView)
            });
            registry.Register(new MappingConfiguration()
            {
                RouteName = "Silver",
                ViewMode = ViewMode.Single,
                ViewType = nameof(SilverView)
            });

            registry.Register(new MappingConfiguration<WithPopupViewModel>()
            {
                RouteName = "PopupZone",
                ViewType = nameof(WithPopupView)
            });
            registry.Register(new MappingConfiguration<WithPopupViewModel>()
            {
                ViewType = nameof(PopupContentView)
            });
        }

        /// <inheritdoc />
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var proto = ContainerProvider.Resolve<IPrototypeInterface>();
            var viewNavigator = ContainerProvider.Resolve<IZoneNavigator>();

            proto?.Print();
            MainWindow = ContainerProvider.Resolve<MainShell>();
            MainWindow?.Show();

            viewNavigator?.Navigate("SingleViewZone", "Single");
            viewNavigator?.Navigate("SwitchableViewZone", typeof(SwitchViewModel));
            viewNavigator?.Navigate("InjectableViewZone", "Injection", typeof(RegionableViewModel));
            viewNavigator?.Navigate("PopupViewZone", "PopupZone", typeof(WithPopupViewModel));
        }
    }
}
