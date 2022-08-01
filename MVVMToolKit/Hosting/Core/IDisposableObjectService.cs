using System;

namespace MVVMToolKit.Hosting.Core
{
    public interface IDisposableObjectService : IDisposable
    {
        void Add(IDisposableObject disposable);
        bool Exists(Guid guid);
        void Remove(IDisposableObject disposable);
    }
}
