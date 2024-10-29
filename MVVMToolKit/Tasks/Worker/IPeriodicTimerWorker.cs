using System.Threading.Tasks;

namespace MVVMToolKit.Tasks.Worker
{
    public interface IPeriodicTimerWorker : IDisposable
    {
        TimeSpan Cycle { get; set; }
        void StartWorker(TimeSpan timespan, Func<Task> work);
        void StopWorker();
    }
}
