using System;
using System.Collections.Generic;
using MVVMToolKit.Hosting.Core;

namespace MVVMToolKit.Hosting.Internal
{
    public class DisposableObjectService : IDisposableObjectService    {
        private readonly DisposableList<IDisposable> disposableList = new DisposableList<IDisposable>();
        private readonly Dictionary<Guid, IDisposable> disposableDictionary = new Dictionary<Guid, IDisposable>();
        private bool disposedValue;

        public void Add(IDisposableObject disposable)
        {
            disposable.Guid = Guid.NewGuid();
            this.disposableDictionary.Add(disposable.Guid, disposable);
            this.disposableList.Add(disposable);
        }

        public bool Exists(Guid guid)
        {
            return this.disposableDictionary.ContainsKey(guid);
        }

        public void Remove(IDisposableObject disposable)
        {
            if (!this.Exists(disposable.Guid))
            {
                return;
            }

            if (this.disposableList.Remove(disposable))
            {
                this.disposableDictionary.Remove(disposable.Guid);
            }

        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    // TODO: 관리형 상태(관리형 개체)를 삭제합니다.
                }
                this.disposableList.Dispose();
                this.disposableDictionary.Clear();
                // TODO: 비관리형 리소스(비관리형 개체)를 해제하고 종료자를 재정의합니다.
                // TODO: 큰 필드를 null로 설정합니다.
                this.disposedValue = true;
            }
        }

        // // TODO: 비관리형 리소스를 해제하는 코드가 'Dispose(bool disposing)'에 포함된 경우에만 종료자를 재정의합니다.
         ~DisposableObjectService()
         {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            this.Dispose(disposing: false);
         }

        public void Dispose()
        {
            // 이 코드를 변경하지 마세요. 'Dispose(bool disposing)' 메서드에 정리 코드를 입력합니다.
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
