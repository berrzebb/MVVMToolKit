using System;
using Microsoft.Extensions.DependencyInjection;

namespace MVVMToolKit.Ioc
{
    /// <summary>
    /// `ContainerProvider`는 서비스 제공자를 통해 특정 타입의 서비스를 해결하는 클래스입니다.<br/>
    /// </summary>
    public static class ContainerProvider
    {
        /// <summary>
        /// `Provider`는 서비스 제공자를 저장하는 필드입니다.<br/>
        /// </summary>
        private static IServiceProvider? Provider;

        internal static void Initialize(IServiceProvider? provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// `Resolve` 메서드는 주어진 타입의 서비스를 해결하고 반환합니다.<br/>
        /// <param name="type">해결할 서비스의 타입입니다.</param>
        /// <param name="serviceKey">해결할 서비스를 찾을 수 있는 키입니다.</param>
        /// <returns>해결된 서비스 객체를 반환합니다. 서비스를 해결할 수 없는 경우 null을 반환합니다.</returns>
        /// </summary>
        public static object? Resolve(Type? type, object? serviceKey = null)
        {
            object? instance = default;
            if (type == null || Provider == null)
            {
                return instance;
            }

            if (Provider is IKeyedServiceProvider requiredServiceSupportingProvider)
            {
                instance = serviceKey is null
                    ? requiredServiceSupportingProvider.GetService(type)
                    : requiredServiceSupportingProvider.GetKeyedService(type, serviceKey);
            }
            else
            {

                instance = Provider.GetService(type);
            }

            return instance;
        }

        /// <summary>
        /// `Resolve{T}` 메서드는 주어진 타입의 서비스를 해결하고 반환합니다.<br/>
        /// <typeparam name="T">해결할 서비스의 타입입니다.</typeparam>
        /// <param name="serviceKey">해결할 서비스를 찾을 수 있는 키입니다.</param>
        /// <returns>해결된 서비스 객체를 반환합니다. 서비스를 해결할 수 없는 경우 null을 반환합니다.</returns>
        /// </summary>
        public static T? Resolve<T>(object? serviceKey = null)
        {
            if (Provider == null)
            {
                return default;
            }

            return (T?)Resolve(typeof(T), serviceKey);
        }
        /// <summary>
        /// 주어진 타입의 지연된 생성이 가능한 객체를 반환합니다.
        /// </summary>
        /// <param name="type">해결할 서비스의 타입입니다.</param>
        /// <param name="serviceKey">해결할 서비스를 찾을 수 있는 키입니다.</param>
        /// <returns>지연된 생성이 가능한 해결된 객체를 반환합니다.</returns>
        public static Lazy<object?> ResolveLazy(Type? type, object? serviceKey = null)
        {
            return new Lazy<object?>(() => Resolve(type, serviceKey));
        }
        /// <summary>
        /// 주어진 타입의 지연된 생성이 가능한 객체를 반환합니다.
        /// </summary>
        /// <typeparam name="T">해결할 서비스의 타입입니다.</typeparam>
        /// <param name="serviceKey">해결할 서비스를 찾을 수 있는 키입니다.</param>
        /// <returns>지연된 생성이 가능한 해결된 객체를 반환합니다.</returns>
        public static Lazy<T?> ResolveLazy<T>(object? serviceKey = null)
        {
            return new Lazy<T?>(() => Resolve<T>(serviceKey));
        }
    }
}
