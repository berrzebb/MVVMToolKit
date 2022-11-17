using System;

namespace MVVMToolKit.Hosting.Core
{
    public interface IWPFViewModel : IDisposableObject
    {
        void InitializeDependency(IServiceProvider containerProvider);
    }
}
