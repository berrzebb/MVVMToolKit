using Prism.Ioc;
using System;

namespace MVVMToolKit.Hosting.Core
{
    public interface IWPFViewModel : IDisposable
    {
        void InitializeDependency(IContainerProvider containerProvider);
    }
}
