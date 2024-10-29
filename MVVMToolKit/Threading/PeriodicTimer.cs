

using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Sources;

namespace MVVMToolKit.Threading
{
    /// <summary>
    /// Periodic Timer
    /// </summary>
    public class PeriodicTimer : IDisposable
    {
        private const uint MaxSupportedTimeout = 4294967294;
        private readonly Timer _timer;
        private readonly State _state;
        /// <summary>
        /// Periodic Timer 생성자
        /// </summary>
        /// <param name="period">주기</param>
        /// <exception cref="ArgumentOutOfRangeException">범위가 벗어났을때 발생</exception>
        public PeriodicTimer(TimeSpan period)
        {
            long ms = (long)period.TotalMilliseconds;
            if (ms < 1 || ms > MaxSupportedTimeout)
            {
                throw new ArgumentOutOfRangeException(nameof(period));
            }

            _state = new State();
            _timer = new Timer(s => ((State)s!).Signal(false), _state, (uint)ms, (uint)ms);

        }
        /// <summary>
        /// 다음 주기를 기다립니다.
        /// </summary>
        /// <param name="cancellationToken">취소 가능한 토큰.</param>
        /// <returns>다음 주기 작업이 가능한지 여부.</returns>
        public ValueTask<bool> WaitForNextTickAsync(CancellationToken cancellationToken) =>
            _state.WaitForNextTickAsync(cancellationToken);
        /// <summary>
        /// 객체를 해제합니다.
        /// </summary>

        /// <summary>
        /// PeriodicTimer 소멸자
        /// </summary>
        ~PeriodicTimer() => Dispose();
        private sealed class State : IValueTaskSource<bool>
        {
            private ManualResetValueTaskSourceCore<bool> _manualResetValueTaskSourceCore = new();

            private CancellationTokenRegistration _ctr;

            private bool _stopped;
            private bool _signaled;
            private bool _activeWait;

            private static readonly object syncRoot = new();
            public ValueTask<bool> WaitForNextTickAsync(CancellationToken cancellationToken)
            {
                lock (syncRoot)
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

                    _activeWait = true;
                    _ctr = cancellationToken.Register(v =>
                    {
                        if (v is KeyValuePair<State, CancellationToken> kv)
                        {
                            kv.Key.Signal(false, cancellationToken: kv.Value);
                        }
                    }, new KeyValuePair<State, CancellationToken>(this, cancellationToken));

                    return new ValueTask<bool>(this, _manualResetValueTaskSourceCore.Version);
                }
            }

            public void Signal(bool stopping) => Signal(stopping, cancellationToken: default);

            public void Signal(bool stopping, CancellationToken cancellationToken)
            {
                bool completeTask = false;

                lock (syncRoot)
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
                        _manualResetValueTaskSourceCore.SetException(new OperationCanceledException(cancellationToken));
                    }
                    else
                    {
                        Debug.Assert(!Monitor.IsEntered(this));
                        _manualResetValueTaskSourceCore.SetResult(true);
                    }
                }
            }

            bool IValueTaskSource<bool>.GetResult(short token)
            {
                _ctr.Dispose();
                lock (syncRoot)
                {
                    try
                    {
                        _manualResetValueTaskSourceCore.GetResult(token);
                    }
                    finally
                    {
                        _manualResetValueTaskSourceCore.Reset();
                        _ctr = default;
                        _activeWait = false;
                        if (!_stopped)
                        {
                            _signaled = false;
                        }
                    }

                    return !_stopped;
                }
            }

            ValueTaskSourceStatus IValueTaskSource<bool>.GetStatus(short token) => _manualResetValueTaskSourceCore.GetStatus(token);

            void IValueTaskSource<bool>.OnCompleted(Action<object?> continuation, object? state, short token, ValueTaskSourceOnCompletedFlags flags) =>
                _manualResetValueTaskSourceCore.OnCompleted(continuation, state, token, flags);
        }
        /// <summary>
        /// 객체 해제
        /// </summary>
        /// <param name="disposing">해제 여부</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                _timer.Dispose();
                _state.Signal(stopping: true);
            }
        }

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
