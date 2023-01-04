using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolKit.Threading
{
    /// <summary>
    /// The cancelable task class
    /// </summary>
    /// <seealso cref="IDisposable"/>
    public class CancelableTask : IDisposable
    {
        /// <summary>
        /// The token source.
        /// </summary>
        private CancellationTokenSource tokenSource;
        
        /// <summary>
        /// The token.
        /// </summary>
        private CancellationToken token;
        
        /// <summary>
        /// The task.
        /// </summary>
        private Task? task;
        
        /// <summary>
        /// The disposed value.
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="CancelableTask"/> class.
        /// </summary>
        public CancelableTask()
        {
            this.tokenSource = new CancellationTokenSource();
            this.token = this.tokenSource.Token;
        }
        
        /// <summary>
        /// Release CancelableTask.
        /// </summary>
        ~CancelableTask()
        {
            this.Dispose(true);
        }
        
        /// <summary>
        /// Gets the value of the is completed.
        /// </summary>
        public bool? IsCompleted => this.task?.IsCompleted;
        
        /// <summary>
        /// Resets this instance.
        /// </summary>
        public void Reset()
        {
            this.Cancel();
            this.tokenSource = new CancellationTokenSource();
            this.token = this.tokenSource.Token;
        }
        /// <summary>
        /// Runs the request task.
        /// </summary>
        /// <param name="requestTask">The request task</param>
        public async Task Run(Action<CancellationToken> requestTask)
        {
            try
            {
                this.task = Task.Run(
                    () => requestTask.Invoke(this.token), 
                    this.token);
                await this.task;
            } catch (TaskCanceledException ex)
            {
                Debug.WriteLine($"Task Execution Cancelled: {ex.Message}");

            }
            
        }
        
        /// <summary>
        /// Cancels this instance.
        /// </summary>
        public void Cancel()
        {
            try
            {
                this.tokenSource.Cancel();
                // this.token.ThrowIfCancellationRequested();
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine($"Task Execution Cancelled: {ex.Message}");
            }
            catch (ObjectDisposedException ex)
            {
                Debug.WriteLine($"Task Execution Cancelled: {ex.Message}");
            }

        }

    /// <summary>
    /// Disposes the disposing,
    /// </summary>
    /// <param name="disposing">The disposing,</param>
    protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                    this.Cancel();
                }

                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                this.disposedValue = true;
            }
        }

    /// <summary>
    /// Disposes this instance,
    /// </summary>
    public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
