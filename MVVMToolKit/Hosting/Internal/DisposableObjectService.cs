using System.Collections.Generic;
using MVVMToolKit.Hosting.Core;

namespace MVVMToolKit.Hosting.Internal
{
    /// <summary>
    /// The disposable object service class
    /// </summary>
    /// <seealso cref="IDisposableObjectService"/>
    public sealed class DisposableObjectService : IDisposableObjectService
    {
        /// <summary>
        /// The disposable
        /// </summary>
        private readonly DisposableList<IDisposable> disposableList = new();
        /// <summary>
        /// The disposable
        /// </summary>
        private readonly Dictionary<Guid, IDisposable> disposableDictionary = new();
        /// <summary>
        /// The disposed value
        /// </summary>
        private bool disposedValue;

        /// <summary>
        /// Adds the disposable
        /// </summary>
        /// <param name="disposable">The disposable</param>
        public void Add(IDisposableObject disposable)
        {
            disposable.Guid = Guid.NewGuid();
            disposableDictionary.Add(disposable.Guid, disposable);
            disposableList.Add(disposable);
        }

        /// <summary>
        /// Describes whether this instance exists
        /// </summary>
        /// <param name="guid">The guid</param>
        /// <returns>The bool</returns>
        public bool Exists(Guid guid)
        {
            return disposableDictionary.ContainsKey(guid);
        }

        /// <summary>
        /// Removes the disposable
        /// </summary>
        /// <param name="disposable">The disposable</param>
        public void Remove(IDisposableObject disposable)
        {
            if (!Exists(disposable.Guid))
            {
                return;
            }

            if (disposableList.Remove(disposable))
            {
                disposableDictionary.Remove(disposable.Guid);
            }

        }

        /// <summary>
        /// Disposes the disposing
        /// </summary>
        /// <param name="disposing">The disposing</param>
        private void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }
                disposableList.Dispose();
                disposableDictionary.Clear();
                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
        ~DisposableObjectService()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: false);
        }

        /// <summary>
        /// Disposes this instance
        /// </summary>
        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
