namespace MVVMToolKit.Hosting.Internal
{
    /// <summary>
    /// The disposable list class
    /// </summary>
    /// <seealso cref="IDisposable"/>
    internal class DisposableList<T> : IDisposable
    {
        /// <summary>
        /// The disposables
        /// </summary>
        private readonly List<IDisposable> _disposables = new();

        /// <summary>
        /// The disposed
        /// </summary>
        private bool _disposed;

        /// <summary>
        /// The lock
        /// </summary>
        private readonly object _lock = new();

        /// <summary>
        /// Adds the item
        /// </summary>
        /// <param name="item">The item</param>
        public void Add(T item)
        {
            if (item is IDisposable disposable)
            {
                lock (_lock)
                {
                    _disposables.Add(disposable);
                }
            }
        }
        /// <summary>
        /// Describes whether this instance remove
        /// </summary>
        /// <param name="item">The item</param>
        /// <returns>The ret</returns>
        public bool Remove(T item)
        {
            bool ret = false;
            if (item is IDisposable disposable)
            {
                lock (_lock)
                {
                    ret = _disposables.Remove(disposable);
                }
            }
            return ret;
        }
        /// <summary>
        /// Disposes this instance
        /// </summary>
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
