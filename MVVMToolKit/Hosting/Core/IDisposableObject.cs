using System;

namespace MVVMToolKit.Hosting.Core
{
    public interface IDisposableObject : IDisposable
    {
        Guid Guid { get; internal set; }
    }
}
