using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolKit.Threading
{
    /// <summary>
    /// The cancelable task class
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public class CancelableTask : ICancelableTask
    {
        private readonly bool _allowConcurrency;
        private Operation? _activeOperation;

        private sealed class Operation : IDisposable
        {
            private readonly CancellationTokenSource _cancellationTokenSource;
#if NET6_0_OR_GREATER
            private readonly TaskCompletionSource _completionSource;
#else
            private readonly TaskCompletionSource<object?> _completionSource;
#endif
            private bool _disposed;

            private readonly object _syncRoot = new();

            public Task Completion => _completionSource.Task;

            public Operation(CancellationTokenSource? cancellationTokenSource = null)
            {
                _cancellationTokenSource = cancellationTokenSource ?? new CancellationTokenSource();
#if NET6_0_OR_GREATER
                _completionSource = new TaskCompletionSource(TaskCreationOptions.RunContinuationsAsynchronously);
#else
                _completionSource = new TaskCompletionSource<object?>(TaskCreationOptions.RunContinuationsAsynchronously);

#endif
            }

            public void Cancel()
            {
                lock (_syncRoot)
                {
                    if (!_disposed) _cancellationTokenSource.Cancel();
                }
            }

            public void Dispose()
            {
                try
                {
                    lock (_syncRoot)
                    {
                        _cancellationTokenSource.Dispose();
                        _disposed = true;
                    }
                }
                finally
                {
#if NET6_0_OR_GREATER
                    _completionSource.SetResult();
#else
                    _completionSource.SetResult(null);
#endif
                }
            }
        }

        public bool IsRunning => Volatile.Read(ref _activeOperation) != null;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelableTask"/> class.
        /// </summary>
        private CancelableTask(bool allowConcurrency)
        {
            _allowConcurrency = allowConcurrency;
        }

        public async Task<TResult> RunAsync<TResult>(
            Func<CancellationToken, Task<TResult>>? action,
            CancellationToken token = default)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(action);
#endif
            CancellationTokenSource cancellationTokenSource = CancellationTokenSource
                .CreateLinkedTokenSource(token);
            using Operation? operation = new(cancellationTokenSource);

            Operation? oldOperation = Interlocked.Exchange(ref _activeOperation, operation);

            try
            {
                if (oldOperation is not null && !_allowConcurrency)
                {
                    oldOperation.Cancel();
                    await oldOperation.Completion; // Continue on captured context
                }

                cancellationTokenSource.Token.ThrowIfCancellationRequested();
                Task<TResult> task = action(cancellationTokenSource.Token);
                return await task.ConfigureAwait(false);
            }
            finally
            {
                Interlocked.CompareExchange(ref _activeOperation, null, operation);
            }
        }

        public Task RunAsync(Func<CancellationToken, Task> action,
            CancellationToken token = default)
        {
#if NET6_0_OR_GREATER
            ArgumentNullException.ThrowIfNull(action);
#endif

            return RunAsync<object?>(async ct =>
            {
                await action(ct).ConfigureAwait(false);
                return null;
            }, token);
        }

        public Task CancelAsync()
        {
            Operation? operation = Volatile.Read(ref _activeOperation);

            if (operation is null) return Task.CompletedTask;

            operation.Cancel();
            return operation.Completion;
        }

        public bool Cancel() => !(CancelAsync().IsCompleted);

        public ICancelableTask Create(bool allowConcurrency = false) => new CancelableTask(allowConcurrency);
    }
}
