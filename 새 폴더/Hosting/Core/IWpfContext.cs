using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading;

namespace MVVMToolKit.Hosting.Core
{
    public interface IWPFContext
    {
        bool IsLifetime { get; internal set; }

        /// <summary>
        /// WPF 응용프로그램이 Microsoft.Extensions.Hosting 내에서 실행 중인지 확인합니다.
        /// </summary>
        bool IsRunning { get; internal set; }
        /// <summary>
        /// WPF 인스턴스 <see cref="Application"/>
        /// </summary>
        Application? WPFApplication { get; }
        /// <summary>
        /// WPF Dispatcher <see cref="WpfApplication"/>
        /// </summary>
        Dispatcher Dispatcher { get; }
    }

    public interface IWPFContext<out TApplication> : IWPFContext where TApplication : Application
    {
        /// <summary>
        /// WPF 인스턴스 <see cref="TApplication"/>.
        /// </summary>
        new TApplication? WPFApplication { get; }
    }
}
