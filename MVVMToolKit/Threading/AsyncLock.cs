using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolKit.Threading
{
    /// <summary>
    /// The async lock class
    /// </summary>
    public sealed class AsyncLock
    {
        /// <summary>
        /// The semaphore
        /// </summary>
        private readonly SemaphoreSlim semaphore;
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLock"/> class
        /// </summary>
        /// <param name="initialCount">The initial count</param>
        /// <param name="maxCount">The max count</param>
        public AsyncLock(int initialCount, int maxCount)
        {
            semaphore = new SemaphoreSlim(initialCount, maxCount);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="AsyncLock"/> class
        /// </summary>
        /// <param name="maxCount">The max count</param>
        public AsyncLock(int maxCount) : this(1, maxCount) { }
        /// <summary>
        /// The handler class
        /// </summary>
        /// <seealso cref="IDisposable"/>
        private sealed class Handler : IDisposable
        {
            /// <summary>
            /// The semaphore
            /// </summary>
            private readonly SemaphoreSlim semaphore;
            /// <summary>
            /// The disposed
            /// </summary>
            private bool _disposed = false;

            /// <summary>
            /// Initializes a new instance of the <see cref="Handler"/> class
            /// </summary>
            /// <param name="semaphore">The semaphore</param>
            public Handler(SemaphoreSlim semaphore)
            {
                this.semaphore = semaphore;
            }
            /// <summary>
            /// Disposes this instance
            /// </summary>
            public void Dispose()
            {
                if (!_disposed)
                {
                    semaphore.Release();
                    _disposed = true;
                }
            }
        }

        /// <summary>
        /// Locks this instance
        /// </summary>
        /// <returns>A task containing the disposable</returns>
        public async Task<IDisposable> LockAsync()
        {
            await semaphore.WaitAsync();
            return new Handler(semaphore);
        }
    }
}
