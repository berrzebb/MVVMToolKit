using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolKit.Threading
{
    public interface ICancelableTask
    {

        bool IsRunning { get; }

        Task<TResult> RunAsync<TResult>(
            Func<CancellationToken, Task<TResult>>? action,
            CancellationToken token = default);

        Task RunAsync(Func<CancellationToken, Task> action,
            CancellationToken token = default);

        Task CancelAsync();

        bool Cancel();
    }
}
