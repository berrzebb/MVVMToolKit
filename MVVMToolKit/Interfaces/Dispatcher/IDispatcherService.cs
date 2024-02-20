using System.Threading;

namespace MVVMToolKit.Interfaces
{
    /// <summary>
    /// UI Dispatcher에 대한 직접 접근을 방지하기 위해 <br/>
    /// Dispatcher를 추상화 한 인터페이스입니다.
    /// </summary>
    public interface IDispatcherService
    {
        /// <summary>
        ///     이 객체에 대한 접근 권한이 호출 스레드에 있는지 확인합니다.
        /// </summary>
        /// <remarks>
        ///     DispatcherObjects에는 디스패처 스레드만 접근할 수 있습니다.
        ///     <p/>
        ///     이 메서드는 public이므로 어떤 스레드든지 DispatcherObject에 접근 권한이 있는지 확인할 수 있습니다.
        /// </remarks>
        /// <returns>
        ///     호출 스레드가 이 객체에 접근 권한이 있으면 true를 반환합니다.
        /// </returns>
        bool CheckAccess();
        /// <summary>
        ///     호출 스레드가 이 객체에 접근할 수 있는지 확인합니다.
        /// </summary>
        /// <remarks>
        ///     DispatcherObjects에는 디스패처 스레드만 접근할 수 있습니다.
        ///     <p/>
        ///     이 메서드는 파생 클래스가 호출 스레드가 자신에게 접근할 수 있는지 확인할 수 있도록 public입니다.
        /// </remarks>

        void VerifyAccess();
        /// <summary>
        ///     Dispatcher가 생성된 스레드에서 지정된 Action을 동기적으로 실행합니다.
        /// </summary>
        /// <param name="callback">
        ///     디스패처를 통해 호출할 Action 대리자입니다.
        /// </param>
        /// <param name="priority">
        ///     지정된 콜백이 디스패처의 다른 보류 중인 작업에 대해 어떤 순서로 호출되는지 결정하는 우선순위입니다.
        /// </param>
        /// <param name="cancellationToken">
        ///     작업을 취소하는 데 사용할 수 있는 취소 토큰입니다.
        ///     작업이 시작되지 않은 경우, 취소 토큰이 취소되면 작업이 중단됩니다. 
        ///     작업이 시작된 경우, 작업은 취소 요청과 협력할 수 있습니다.
        /// </param>
        /// <param name="timeout">
        ///     작업이 시작되기까지 기다릴 최소 시간입니다.
        ///     작업이 시작되면, 이 메서드가 반환되기 전에 작업이 완료됩니다.
        /// </param>
        void Invoke(Action? callback, DispatcherPriority? priority = null, CancellationToken? cancellationToken = null, TimeSpan? timeout = null);

        /// <summary>
        ///     Dispatcher가 생성된 스레드에서 지정된 Func&lt;TResult&gt;를 동기적으로 실행합니다.
        /// </summary>
        /// <param name="callback">
        ///     디스패처를 통해 호출할 Func&lt;TResult&gt; 대리자입니다.
        /// </param>
        /// <param name="priority">
        ///     지정된 콜백이 디스패처의 다른 보류 중인 작업에 대해 어떤 순서로 호출되는지 결정하는 우선순위입니다.
        /// </param>
        /// <param name="cancellationToken">
        ///     작업을 취소하는 데 사용할 수 있는 취소 토큰입니다.
        ///     작업이 시작되지 않은 경우, 취소 토큰이 취소되면 작업이 중단됩니다. 
        ///     작업이 시작된 경우, 작업은 취소 요청과 협력할 수 있습니다.
        /// </param>
        /// <param name="timeout">
        ///     작업이 시작되기까지 기다릴 최소 시간입니다.
        ///     작업이 시작되면, 이 메서드가 반환되기 전에 작업이 완료됩니다.
        /// </param>
        /// <returns>
        ///     호출된 대리자의 반환 값입니다.
        /// </returns>
        TResult? Invoke<TResult>(Func<TResult>? callback, DispatcherPriority? priority = null,
            CancellationToken? cancellationToken = null, TimeSpan? timeout = null);

        /// <summary>
        ///     Dispatcher가 생성된 스레드에서 지정된 Action을 비동기적으로 실행합니다.
        /// </summary>
        /// <param name="callback">
        ///     디스패처를 통해 호출할 Action 대리자입니다.
        /// </param>
        /// <param name="priority">
        ///     지정된 콜백이 디스패처의 다른 보류 중인 작업에 대해 어떤 순서로 호출되는지 결정하는 우선순위입니다.
        /// </param>
        /// <param name="cancellationToken">
        ///     작업을 취소하는 데 사용할 수 있는 취소 토큰입니다.
        ///     작업이 시작되지 않은 경우, 취소 토큰이 취소되면 작업이 중단됩니다. 
        ///     작업이 시작된 경우, 작업은 취소 요청과 협력할 수 있습니다.
        /// </param>
        /// <returns>
        ///     호출될 대기 중인 대리자를 나타내는 작업을 반환합니다.
        /// </returns>
        DispatcherOperation InvokeAsync(Action? callback, DispatcherPriority? priority = null, CancellationToken? cancellationToken = null);

        /// <summary>
        ///     Dispatcher가 생성된 스레드에서 지정된 Func&lt;TResult&gt;를 비동기적으로 실행합니다.
        /// </summary>
        /// <param name="callback">
        ///     디스패처를 통해 호출할 Func&lt;TResult&gt; 대리자입니다.
        /// </param>
        /// <param name="priority">
        ///     지정된 콜백이 디스패처의 다른 보류 중인 작업에 대해 어떤 순서로 호출되는지 결정하는 우선순위입니다.
        /// </param>
        /// <param name="cancellationToken">
        ///     작업을 취소하는 데 사용할 수 있는 취소 토큰입니다.
        ///     작업이 시작되지 않은 경우, 취소 토큰이 취소되면 작업이 중단됩니다. 
        ///     작업이 시작된 경우, 작업은 취소 요청과 협력할 수 있습니다.
        /// </param>
        /// <returns>
        ///     호출될 대기 중인 대리자를 나타내는 작업을 반환합니다.
        /// </returns>
        DispatcherOperation<TResult> InvokeAsync<TResult>(Func<TResult>? callback, DispatcherPriority? priority = null, CancellationToken? cancellationToken = null);
    }
}
