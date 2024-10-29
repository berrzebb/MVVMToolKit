

using System.Windows.Markup;

namespace MVVMToolKit.Ioc
{
    /// <summary>
    /// Container Provider를 Markup에서 사용할 수 있는 확장입니다.
    /// </summary>
    /// <seealso cref="MarkupExtension"/>
    public class ContainerProviderExtension : MarkupExtension
    {
        /// <summary>
        ///<see cref="ContainerProviderExtension"/> 생성자
        /// </summary>
        public ContainerProviderExtension()
        {
        }

        /// <summary>
        /// <see cref="ContainerProviderExtension"/> 생성자
        /// </summary>
        /// <param name="type">해결할 타입</param>
        /// <param name="serviceKey">타입을 찾아오기 위한 키(Optional)</param>
        public ContainerProviderExtension(Type? type, object? serviceKey)
        {
            Type = type;
            ServiceKey = serviceKey;
        }
        /// <summary>
        ///  <see cref="ContainerProviderExtension"/> 생성자
        /// </summary>
        /// <param name="type">해결할 타입</param>
        public ContainerProviderExtension(Type? type) : this(type, null)
        {
        }
        /// <summary>
        /// 해결할 타입
        /// </summary>
        public Type? Type { get; }
        /// <summary>
        /// 타입을 찾기 위한 키
        /// </summary>
        public object? ServiceKey { get; }

        /// <summary>
        ///  <see cref="ContainerProvider"/>를 통해 객체를 찾아옵니다.
        /// </summary>
        /// <param name="serviceProvider">DI 컨테이너</param>
        /// <returns>찾은 객체</returns>
        public override object? ProvideValue(IServiceProvider serviceProvider)
        {
            return ContainerProvider.Resolve(Type, ServiceKey);
        }
    }
}
