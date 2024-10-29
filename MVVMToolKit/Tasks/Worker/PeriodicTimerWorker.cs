using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PeriodicTimer = MVVMToolKit.Threading.PeriodicTimer;

namespace MVVMToolKit.Tasks.Worker
{
    public class PeriodicTimerWorker : IPeriodicTimerWorker
    {
        private string _timerName;
        private PeriodicTimer? _periodicTimer;
        private CancellationTokenSource? _cts;
        private Func<Task> _work;

        private Task? periodicTask;
        private ILogger? _logger;

        private TimeSpan _cycle;
        public TimeSpan Cycle
        {
            get { return _cycle; }
            set
            {

                if (_cycle != value && _cts != null)
                {
                    StopWorker();
                    StartWorker(value, _work);
                }
                _cycle = value;
            }
        }

        internal PeriodicTimerWorker(string timerName, ILogger? logger = null)
        {
            _timerName = timerName;
            _logger = logger;

        }
        public void StartWorker(TimeSpan cycle, Func<Task> work)
        {
            _periodicTimer = new PeriodicTimer(cycle);
            _work = work;

            if (_cts == null || _cts?.IsCancellationRequested == true)
            {
                _cts = new CancellationTokenSource();
                periodicTask = Task.Factory.StartNew(_ => ActionPeriodicTask(_cts.Token), TaskCreationOptions.LongRunning);
                _logger?.LogInformation($"{_timerName} Start");
            }
        }
        public void StopWorker()
        {
            Task.Run(StopAndReleaseTask);
        }

        private async Task StopAndReleaseTask()
        {
            if (_cts?.IsCancellationRequested == false)
            {
                _cts.Cancel();
                if (periodicTask != null)
                    await periodicTask;
                _cts?.Dispose();
                _periodicTimer?.Dispose();
                _cts = null;
                _periodicTimer = null;
            }
        }
        private async void ActionPeriodicTask(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    if (_periodicTimer != null && await _periodicTimer.WaitForNextTickAsync(token).ConfigureAwait(false))
                    {
                        if (_work != null) await _work().ConfigureAwait(false);
                    }
                }
            }
            catch (OperationCanceledException oce)
            {
                _logger?.LogInformation(oce, "{TimerName} {Message}", _timerName, oce.Message);
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "{TimerName} {Message}", this._timerName, ex.ToString());
            }

            _logger?.LogInformation("{TimerName} Finished", _timerName);
        }

        public void Dispose()
        {
            StopWorker();
        }
    }
}
