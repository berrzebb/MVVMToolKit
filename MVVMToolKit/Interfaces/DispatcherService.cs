using System.Threading;

namespace MVVMToolKit.Interfaces
{
    public class DispatcherService : IDispatcherService
    {
        private readonly Dispatcher dispatcher;

        public DispatcherService() : this(Application.Current.Dispatcher)
        {

        }

        public DispatcherService(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher;
        }

        public bool CheckAccess()
        {
            return dispatcher.CheckAccess();
        }

        public void VerifyAccess()
        {
            dispatcher.VerifyAccess();
        }

        public void Invoke(Action? callback, DispatcherPriority? priority = null,
            CancellationToken? cancellationToken = null, TimeSpan? timeout = null)
        {
            if (callback is null) return;
            if (CheckAccess())
            {
                callback();
            }
            else
            {
                DispatcherPriority _priority = priority ?? DispatcherPriority.Send;
                CancellationToken _cancellationToken = cancellationToken ?? CancellationToken.None;
                TimeSpan _timeout = timeout ?? TimeSpan.FromMilliseconds(-1);

                dispatcher.Invoke(callback, _priority, _cancellationToken, _timeout);
            }
        }

        public TResult? Invoke<TResult>(Func<TResult>? callback, DispatcherPriority? priority = null,
            CancellationToken? cancellationToken = null, TimeSpan? timeout = null)
        {
            if (callback is null) return default;
            if (CheckAccess())
            {
                return callback();
            }

            DispatcherPriority _priority = priority ?? DispatcherPriority.Send;
            CancellationToken _cancellationToken = cancellationToken ?? CancellationToken.None;
            TimeSpan _timeout = timeout ?? TimeSpan.FromMilliseconds(-1);

            return dispatcher.Invoke(callback, _priority, _cancellationToken, _timeout);
        }

        public DispatcherOperation InvokeAsync(Action? callback, DispatcherPriority? priority = null,
            CancellationToken? cancellationToken = null)
        {
            DispatcherPriority _priority = priority ?? DispatcherPriority.Normal;
            CancellationToken _cancellationToken = cancellationToken ?? CancellationToken.None;

            return dispatcher.InvokeAsync(callback, _priority, _cancellationToken);
        }

        public DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult>? callback,
            DispatcherPriority? priority = null, CancellationToken? cancellationToken = null)
        {
            DispatcherPriority _priority = priority ?? DispatcherPriority.Normal;
            CancellationToken _cancellationToken = cancellationToken ?? CancellationToken.None;

            return dispatcher.InvokeAsync(callback, _priority, _cancellationToken);
        }
    }
}

