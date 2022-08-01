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
            if (item is IDisposable disposable)
            {
                lock (this._lock)
                {
                    this._disposables.Add(disposable);
                }
            }
        }
        public bool Remove(T item)
        {
            bool ret = false;
            if (item is IDisposable disposable)
            {
                lock (this._lock)
                {
                    ret = this._disposables.Remove(disposable);
                }
            }
            return ret;
        }
        public void Dispose()
        {
            lock (this._lock)
            {
                if (this._disposed)
                {
                    return;
                }

                this._disposed = true;
                for (var i = this._disposables.Count - 1; i >= 0; i--)
                {
                    var disposable = this._disposables[i];
                    disposable.Dispose();
                }
                this._disposables.Clear();
            }
        }
    }
}
