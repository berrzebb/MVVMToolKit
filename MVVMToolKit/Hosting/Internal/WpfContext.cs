using MVVMToolKit.Hosting.Core;
using System;
using System.Windows;
using System.Windows.Threading;

namespace MVVMToolKit.Hosting.Internal
{
    internal class WPFContext<TApplication> : IWPFContext<TApplication> where TApplication : Application
    {
        private TApplication? _wpfApplication;
        private bool _isLifeTime;
        private bool _isRunning;
        private ExitEventHandler? _exitEventHandler;
        public bool IsLifeTime
        {
            get => _isLifeTime;
            internal set => _isLifeTime = value;
        }
        /// <summary>
        /// WPF 응용프로그램이 Microsoft.Extensions.Hosting 내에서 실행 중인지 확인합니다.
        /// </summary>
        public bool IsRunning
        {
            get => _isRunning;
            internal set => _isRunning = value;
        }
        public ExitEventHandler ExitHandler
        {
            get => _exitEventHandler!;
            set => _exitEventHandler = value;
        }
        /// <inheritdoc/>
        bool IWPFContext.IsLifetime
        {
            get => _isLifeTime;
            set => _isLifeTime = value;
        }
        /// <inheritdoc/>
        bool IWPFContext.IsRunning
        {
            get => _isRunning;
            set => _isRunning = value;
        }
        /// <inheritdoc/>
        Application? IWPFContext.WPFApplication => _wpfApplication;
        /// <inheritdoc/>
        public TApplication? WPFApplication
        {
            get => _wpfApplication;
            private set => _wpfApplication = value;
        }

        public Dispatcher Dispatcher => _wpfApplication?.Dispatcher ?? throw new InvalidOperationException($"{nameof(WPFApplication)} is not initialized!");

        internal void SetWPFApplication(TApplication application)
        {
            _wpfApplication = application;
        }
    }
}
