using System.Threading;
using MVVMToolKit.Interfaces;

namespace MVVMToolKit.Threading.UI
{
    public class DispatcherService : IDispatcherService
    {
        private readonly Dispatcher _dispatcher;

        public DispatcherService() : this(Application.Current.Dispatcher)
        {

        }

        public DispatcherService(Dispatcher dispatcher) => _dispatcher = dispatcher;

        public bool CheckAccess() => _dispatcher.CheckAccess();

        public void VerifyAccess() => _dispatcher.VerifyAccess();

        public void Invoke(Action? callback, DispatcherPriority? priority = null,
                           CancellationToken? cancellationToken = null, TimeSpan? timeout = null)
        {
            if (callback is null) return;
            if (CheckAccess())
                callback();
            else
            {
                DispatcherPriority dispatcherPriority = priority ?? DispatcherPriority.Send;
                CancellationToken token = cancellationToken ?? CancellationToken.None;
                TimeSpan milliseconds = timeout ?? TimeSpan.FromMilliseconds(-1);

                _dispatcher.Invoke(callback, dispatcherPriority, token, milliseconds);
            }
        }

        public TResult? Invoke<TResult>(Func<TResult>? callback, DispatcherPriority? priority = null,
            CancellationToken? cancellationToken = null, TimeSpan? timeout = null)
        {
            if (callback is null) return default;
            if (CheckAccess())
                return callback();

            DispatcherPriority dispatcherPriority = priority ?? DispatcherPriority.Send;
            CancellationToken token = cancellationToken ?? CancellationToken.None;
            TimeSpan milliseconds = timeout ?? TimeSpan.FromMilliseconds(-1);

            return _dispatcher.Invoke(callback, dispatcherPriority, token, milliseconds);
        }

        public DispatcherOperation InvokeAsync(Action? callback, DispatcherPriority? priority = null,
            CancellationToken? cancellationToken = null)
        {
            DispatcherPriority dispatcherPriority = priority ?? DispatcherPriority.Normal;
            CancellationToken token = cancellationToken ?? CancellationToken.None;

            return _dispatcher.InvokeAsync(callback, dispatcherPriority, token);
        }

        public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult>? callback,
            DispatcherPriority? priority = null, CancellationToken? cancellationToken = null)
        {
            DispatcherPriority dispatcherPriority = priority ?? DispatcherPriority.Normal;
            CancellationToken token = cancellationToken ?? CancellationToken.None;

            return _dispatcher.InvokeAsync(callback, dispatcherPriority, token);
        }
    }
}

