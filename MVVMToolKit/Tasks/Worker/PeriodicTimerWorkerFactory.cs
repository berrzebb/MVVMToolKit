namespace MVVMToolKit.Tasks.Worker
{
    using Microsoft.Extensions.Logging;

    public static class PeriodicTimerWorkerFactory
    {
        public static IPeriodicTimerWorker Create(string timerName, ILogger? logger = null)
        {
            return new PeriodicTimerWorker(timerName, logger);
        }
    }
}
