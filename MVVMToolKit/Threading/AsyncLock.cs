using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolKit.Threading
{
    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim semaphore;
        public AsyncLock(int initialCount, int maxCount)
        {
            this.semaphore = new SemaphoreSlim(initialCount, maxCount);
        }
        public AsyncLock(int maxCount) : this(1, maxCount) { } 
        private sealed class Handler : IDisposable
        {
            private readonly SemaphoreSlim semaphore;
            private bool _disposed = false;

            public Handler(SemaphoreSlim semaphore)
            {
                this.semaphore = semaphore;
            }
            public void Dispose()
            {
                if (!this._disposed)
                {
                    this.semaphore.Release();
                    this._disposed = true;
                }
            }
        }

        public async Task<IDisposable> LockAsync()
        {
            await this.semaphore.WaitAsync();
            return new Handler(this.semaphore); 
        }
    }
}
