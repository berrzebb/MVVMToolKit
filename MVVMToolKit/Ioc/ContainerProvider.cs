namespace MVVMToolKit.Ioc
{
    using Microsoft.Extensions.DependencyInjection;

    /// <summary>
    /// `ContainerProvider`�� ���� �����ڸ� ���� Ư�� Ÿ���� ���񽺸� �ذ��ϴ� Ŭ�����Դϴ�.<br/>
    /// </summary>
    public static class ContainerProvider
    {
        /// <summary>
        /// `Provider`�� ���� �����ڸ� �����ϴ� �ʵ��Դϴ�.<br/>
        /// </summary>
        private static IServiceProvider? Provider;

        internal static void Initialize(IServiceProvider? provider)
        {
            Provider = provider;
        }

        /// <summary>
        /// `Resolve` �޼���� �־��� Ÿ���� ���񽺸� �ذ��ϰ� ��ȯ�մϴ�.<br/>
        /// <param name="type">�ذ��� ������ Ÿ���Դϴ�.</param>
        /// <param name="serviceKey">�ذ��� ���񽺸� ã�� �� �ִ� Ű�Դϴ�.</param>
        /// <returns>�ذ�� ���� ��ü�� ��ȯ�մϴ�. ���񽺸� �ذ��� �� ���� ��� null�� ��ȯ�մϴ�.</returns>
        /// </summary>
        public static object? Resolve(Type? type, object? serviceKey = null)
        {
            if (type == null || Provider == null)
            {
                return null;
            }

            return serviceKey is null ? Provider.GetRequiredService(type) : Provider.GetRequiredKeyedService(type, serviceKey);
        }

        /// <summary>
        /// `Resolve{T}` �޼���� �־��� Ÿ���� ���񽺸� �ذ��ϰ� ��ȯ�մϴ�.<br/>
        /// <typeparam name="T">�ذ��� ������ Ÿ���Դϴ�.</typeparam>
        /// <param name="serviceKey">�ذ��� ���񽺸� ã�� �� �ִ� Ű�Դϴ�.</param>
        /// <returns>�ذ�� ���� ��ü�� ��ȯ�մϴ�. ���񽺸� �ذ��� �� ���� ��� null�� ��ȯ�մϴ�.</returns>
        /// </summary>
        public static T? Resolve<T>(object? serviceKey = null)
        {
            if (Provider == null)
            {
                return default;
            }

            return (T?)Resolve(typeof(T), serviceKey);
        }
    }
}
