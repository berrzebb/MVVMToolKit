using MVVMToolKit.Hosting.Extensions;
using MVVMToolKitSample.Views;
using System;

namespace MVVMToolKitSample
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {

        private App() : this(null!)
        {
            this.CheckForInvalidConstructorConfiguration();
        }
        public App(IServiceProvider provider) : base(provider)
        {
            this.CheckForInvalidConstructorConfiguration();
        }
        public override void Initialize()
        {
            InitializeBootstrapper<MainWindow>();
        }
    }
}
