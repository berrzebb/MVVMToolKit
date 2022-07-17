using System;
using System.Collections.Generic;

namespace MVVMToolKit.Hosting.Internal
{
    internal class DisposableList<T> : IDisposable
    {
        private readonly List<IDisposable> _disposables = new();

        private bool _disposed;

        private readonly object _lock = new();

        public void Add(T item)
        {
            CaptureDiposable(item);
        }

        private void CaptureDiposable(T item)
        {
            if (item is IDisposable disposable)
            {
                lock (_lock)
                {
                    _disposables.Add(disposable);
                }
            }
        }
        public void Dispose()
        {
            lock (_lock)
            {
                if (_disposed)
                {
                    return;
                }

                _disposed = true;
                for (var i = _disposables.Count - 1; i >= 0; i--)
                {
                    var disposable = _disposables[i];
                    disposable.Dispose();
                }
                _disposables.Clear();
            }
        }
    }
}
