using Prism.Ioc;

namespace MVVMToolKit.Hosting.Core
{
    public interface IWPFViewModel : IDisposableObject
    {
        void InitializeDependency(IContainerProvider containerProvider);
    }
}
