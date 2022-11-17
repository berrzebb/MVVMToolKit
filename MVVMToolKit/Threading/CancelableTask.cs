using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace MVVMToolKit.Threading
{
    public class CancelableTask : IDisposable
    {
        private CancellationTokenSource tokenSource;
        private CancellationToken token;
        private Task task;
        private bool disposedValue;

        public CancelableTask()
        {
            this.tokenSource = new CancellationTokenSource();
            this.token = this.tokenSource.Token;
        }
        ~CancelableTask()
        {
            this.Dispose(true);
        }
        public bool? IsCompleted => this.task?.IsCompleted;
        public void Reset()
        {
            this.Cancel();
            this.tokenSource = new CancellationTokenSource();
            this.token = this.tokenSource.Token;
        }
        public async Task Run(Action<CancellationToken> requestTask)
        {
            try
            {
                this.task = Task.Run(() => {
                    requestTask.Invoke(this.token);
                }, this.token);
                await this.task;
            } catch(TaskCanceledException ex)
            {
                Debug.WriteLine($"Task Execution Cancelled: {ex.Message}");

            }
        }
        public void Cancel()
        {
            try
            {
                this.tokenSource.Cancel();
                //this.token.ThrowIfCancellationRequested();
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

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        // ~CancelableTask()
        // {
        //     // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
        //     Dispose(disposing: false);
        // }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
