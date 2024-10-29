using System;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolKit.Threading
{
    /// <summary>
    /// 취소가능한 작업 인터페이스
    /// </summary>
    public interface ICancelableTask
    {
        /// <summary>
        ///  작업이 실행중인지 여부.
        /// </summary>
        bool IsRunning { get; }
        /// <summary>
        ///  작업 실행.
        /// </summary>
        /// <typeparam name="TResult">결과 타입.</typeparam>
        /// <param name="action">작업.</param>
        /// <param name="token">취소할 수 있는 토큰.</param>
        /// <returns>작업 결과.</returns>
        Task<TResult> RunAsync<TResult>(
            Func<CancellationToken, Task<TResult>>? action,
            CancellationToken token = default);
        /// <summary>
        /// 작업 실행
        /// </summary>
        /// <param name="action">작업</param>
        /// <param name="token">취소할 수 있는 토큰.</param>
        /// <returns>작업</returns>
        Task RunAsync(Func<CancellationToken, Task> action,
            CancellationToken token = default);

        /// <summary>
        /// 작업 취소.
        /// </summary>
        /// <returns>취소된 작업.</returns>
        Task CancelAsync();
        /// <summary>
        /// 작업 취소
        /// </summary>
        /// <returns>작업이 취소되었는지 여부.</returns>
        bool Cancel();
    }
}
