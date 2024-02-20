using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace MVVMToolKit.Threading
{

    public class PeriodicTimer : IDisposable
    {
        private const uint MaxSupportedTimeout = 4294967294;
        private readonly Timer _timer;
        private readonly State _state;
        public PeriodicTimer(TimeSpan period)
        {
            long ms = (long)period.TotalMilliseconds;
            if (ms < 1 || ms > MaxSupportedTimeout)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            _state = new State();
            _timer = new Timer(s => ((State)s!).Signal(), _state, (uint)ms, (uint)ms);

        }

        public ValueTask<bool> WaitForNextTickAsync(CancellationToken cancellationToken) =>
            _state.WaitForNextTickAsync(this, cancellationToken);

        public void Dispose()
        {
            GC.SuppressFinalize(this);
            _timer.Dispose();
            _state.Signal(stopping: true);
        }

        ~PeriodicTimer() => Dispose();
        private sealed class State : IValueTaskSource<bool>
        {
            private PeriodicTimer? _owner;

            private ManualResetValueTaskSourceCore<bool> _manualResetValueTaskSourceCore;

            private CancellationTokenRegistration _ctr;

            private bool _stopped;
            private bool _signaled;
            private bool _activeWait;

            private static readonly object SyncRoot = new();
            public ValueTask<bool> WaitForNextTickAsync(PeriodicTimer owner, CancellationToken cancellationToken)
            {
                lock (SyncRoot)
                {
                    if (_activeWait)
                    {
                        throw new InvalidOperationException();
                    }

                    if (cancellationToken.IsCancellationRequested)
                    {
                        return new ValueTask<bool>(Task.FromCanceled<bool>(cancellationToken));
                    }

                    if (_signaled)
                    {
                        if (!_stopped)
                        {
                            _signaled = false;
                        }

                        return new ValueTask<bool>(!_stopped);
                    }
                    Debug.Assert(!_stopped, "Unexpectedly stopped without _signaled being true.");

                    _owner = owner;
                    _activeWait = true;
                    _ctr = cancellationToken.Register(v =>
                    {
                        if (v is KeyValuePair<State, CancellationToken> kv)
                        {
                            kv.Key.Signal(cancellationToken: kv.Value);
                        }
                    }, new KeyValuePair<State, CancellationToken>(this, cancellationToken));

                    return new ValueTask<bool>(this, this._manualResetValueTaskSourceCore.Version);
                }
            }

            public void Signal(bool stopping = false, CancellationToken cancellationToken = default(CancellationToken))
            {
                bool completeTask = false;

                lock (SyncRoot)
                {
                    _stopped |= stopping;

                    if (!_signaled)
                    {
                        _signaled = true;
                        completeTask = _activeWait;
                    }
                }

                if (completeTask)
                {
                    if (cancellationToken.IsCancellationRequested)
                    {
                        this._manualResetValueTaskSourceCore.SetException(new OperationCanceledException(cancellationToken));
                    }
                    else
                    {
                        Debug.Assert(!Monitor.IsEntered(this));
                        this._manualResetValueTaskSourceCore.SetResult(true);
                    }
                }
            }

            bool IValueTaskSource<bool>.GetResult(short token)
            {
                _ctr.Dispose();
                lock (SyncRoot)
                {
                    try
                    {
                        this._manualResetValueTaskSourceCore.GetResult(token);
                    }
                    finally
                    {
                        this._manualResetValueTaskSourceCore.Reset();
                        _ctr = default;
                        _activeWait = false;
                        _owner = null;
                        if (!_stopped)
                        {
                            _signaled = false;
                        }
                    }

                    return !_stopped;
                }
            }

            ValueTaskSourceStatus IValueTaskSource<bool>.GetStatus(short token) => this._manualResetValueTaskSourceCore.GetStatus(token);

            void IValueTaskSource<bool>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) =>
                this._manualResetValueTaskSourceCore.OnCompleted(continuation, state, token, flags);
        }
    }
}
