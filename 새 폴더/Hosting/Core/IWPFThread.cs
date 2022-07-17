using System.Threading;
using System.Windows;

namespace MVVMToolKit.Hosting.Core
{
    public interface IWPFThread
    {
        IWPFContext WPFContext { get; }

        Thread MainThread { get; }

        SynchronizationContext SynchronizationContext { get; }

        /// <summary>
        /// WPF Thread를 시작합니다.
        /// </summary>
        void Start();

        /// <summary>
        /// 응용프로그램 종료를 처리합니다.
        /// </summary>
        void HandleApplicationExit();
    }

    public interface IWPFThread<out TApplication> : IWPFThread where TApplication : Application, IApplicationInitializeComponent
    {
        new IWPFContext<TApplication> WPFContext { get; }
    }
}
